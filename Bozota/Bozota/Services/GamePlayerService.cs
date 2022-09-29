using Bozota.Common.Models;
using Bozota.Common.Models.Items;
using Bozota.Common.Models.Items.Abstractions;
using Bozota.Common.Models.Objects;
using Bozota.Common.Models.Players;
using Bozota.Common.Models.Players.Abstractions;

namespace Bozota.Services;

public class GamePlayerService
{
    private readonly ILogger<GamePlayerService> _logger;
    private readonly Random _random = new();
    private readonly int _playerHealth;
    private readonly int _playerMinHealth;
    private readonly int _playerMaxHealth;
    private readonly int _playerSpeed;
    private readonly int _playerMaxSpeed;
    private readonly int _playerStartingAmmo;
    private readonly int _playerStartingMaterials;
    private readonly int _bulletSpeed;
    private readonly int _bulletDamage;
    private readonly int _bombRadius;
    private readonly int _wallHealth;

    public GamePlayerService(ILogger<GamePlayerService> logger, IConfiguration config)
    {
        _logger = logger;

        _playerHealth = config.GetValue("Game:PlayerHealth", 100);
        _playerMinHealth = config.GetValue("Game:PlayerMinHealth", 10);
        _playerMaxHealth = config.GetValue("Game:PlayerMaxHealth", 200);
        _playerSpeed = config.GetValue("Game:PlayerSpeed", 1);
        _playerMaxSpeed = config.GetValue("Game:PlayerMaxSpeed", 5);
        _playerStartingAmmo = config.GetValue("Game:PlayerStartingAmmo", 10);
        _playerStartingMaterials = config.GetValue("Game:PlayerStartingMaterials", 3);
        _bulletSpeed = config.GetValue("Game:BulletSpeed", 3);
        _bulletDamage = config.GetValue("Game:BulletDamage", 40);
        _bombRadius = config.GetValue("Game:BombRadius", 3);
        _wallHealth = config.GetValue("Game:WallHealth", 50);
    }

    public async Task<Player?> AddNewPlayerWithRandomPosition(string name, GameState gameState)
    {
        _logger.LogInformation("Adding new player: {player}", name);

        Player? player = null;

        // Check if randomly assigned position is already taken by other item or object on map
        int tryCounter = 0;
        while (tryCounter <= gameState.TotalCellCount)
        {
            tryCounter++;

            player = await AddNewPlayer(name, gameState, _random.Next(gameState.MapXCellCount), _random.Next(gameState.MapYCellCount));

            if (player == null)
            {
                continue;
            }

            break;
        }

        return player;
    }

    public Task<Player?> AddNewPlayer(string name, GameState gameState, int x, int y)
    {
        // Check if position is already occupied by other item or object on map
        bool isPositionOccupiedByBombOrBombRadius = false;
        for (int bombY = 0 - _bombRadius; bombY <= _bombRadius; ++bombY)
        {
            for (int bombX = 0 - _bombRadius; bombX <= _bombRadius; ++bombX)
            {
                if (!(bombX == _bombRadius && bombY == _bombRadius) &&
                    !(bombX == _bombRadius && bombY == 0 - _bombRadius) &&
                    !(bombX == 0 - _bombRadius && bombY == _bombRadius) &&
                    !(bombX == 0 - _bombRadius && bombY == 0 - _bombRadius) &&
                    IsPositionOccupied(x + bombX, y + bombY, gameState.Bombs))
                {
                        isPositionOccupiedByBombOrBombRadius = true;
                        break;
                }
            }
            if (isPositionOccupiedByBombOrBombRadius) { break; }
        }
        if (isPositionOccupiedByBombOrBombRadius ||
            IsPositionOccupied(x, y, gameState.Walls) ||
            IsPositionOccupied(x, y, gameState.AmmoItems) ||
            IsPositionOccupied(x, y, gameState.Bullets) ||
            IsPositionOccupied(x, y, gameState.FireItems) ||
            IsPositionOccupied(x, y, gameState.HealthItems) ||
            IsPositionOccupied(x, y, gameState.MaterialsItems) ||
            IsPositionOccupied(x, y, gameState.Players) ||
            IsPositionOccupied(x + 1, y, gameState.Players) ||
            IsPositionOccupied(x - 1, y, gameState.Players) ||
            IsPositionOccupied(x, y + 1, gameState.Players) ||
            IsPositionOccupied(x, y - 1, gameState.Players))
        {
            return Task.FromResult<Player?>(null);
        }

        return Task.FromResult<Player?>(new Player(name, x, y, _playerHealth, _playerMinHealth, _playerMaxHealth, _playerSpeed, _playerStartingAmmo, _playerStartingMaterials));
    }

    public static bool IsPositionOccupied<T>(int x, int y, List<T> items) where T : IMapItem
    {
        foreach (T item in items)
        {
            if (item.XPos == x && item.YPos == y)
            {
                return true;
            }
        }

        return false;
    }

    public Task ProcessPlayers(GameState gameState)
    {
        // Remove dead players
        List<Player> deadPlayer = new();
        foreach (Player player in gameState.Players)
        {
            if (!player.Health.IsAlive && !player.Health.IsInDestructable)
            {
                deadPlayer.Add(player);
            }
        }
        foreach (Player player in deadPlayer)
        {
            gameState.Players.Remove(player);
        }

        // Process all player actions
        int actionCounter = 0;
        bool arePlayerActionsRemaining;
        do
        {
            arePlayerActionsRemaining = false;

            foreach (Player player in gameState.Players)
            {
                if (player.Speed - actionCounter > 0)
                {
                    switch (player.Actions.Last())
                    {
                        case { Action: GameAction.Move, Direction: Direction.Up }:
                            if (PlayerPositionIsNotAtUpperBorder(player, gameState.MapYCellCount) &&
                                PositionIsNotOccupiedByObject(player.XPos, player.YPos + 1, gameState))
                            {
                                player.YPos += 1;
                            }
                            break;
                        case { Action: GameAction.Move, Direction: Direction.Right }:
                            if (PlayerPositionIsNotAtRightBorder(player, gameState.MapXCellCount) &&
                                PositionIsNotOccupiedByObject(player.XPos + 1, player.YPos, gameState))
                            {
                                player.XPos += 1;
                            }
                            break;
                        case { Action: GameAction.Move, Direction: Direction.Down }:
                            if (PlayerPositionIsNotAtLowerBorder(player) &&
                                PositionIsNotOccupiedByObject(player.XPos, player.YPos - 1, gameState))
                            {
                                player.YPos -= 1;
                            }
                            break;
                        case { Action: GameAction.Move, Direction: Direction.Left }:
                            if (PlayerPositionIsNotAtLeftBorder(player) &&
                                PositionIsNotOccupiedByObject(player.XPos - 1, player.YPos, gameState))
                            {
                                player.XPos -= 1;
                            }
                            break;
                        case { Action: GameAction.Shoot, Direction: Direction.Up }:
                            if (PlayerPositionIsNotAtUpperBorder(player, gameState.MapYCellCount) &&
                                player.HasEnoughAmmo(1))
                            {
                                gameState.Bullets.Add(new BulletItem(player.XPos, player.YPos + 1, Direction.Up, _bulletSpeed, _bulletDamage, player.Name));
                                player.ReduceAmmo(1);

                            }
                            break;
                        case { Action: GameAction.Shoot, Direction: Direction.Right }:
                            if (PlayerPositionIsNotAtRightBorder(player, gameState.MapXCellCount) &&
                                player.HasEnoughAmmo(1))
                            {
                                gameState.Bullets.Add(new BulletItem(player.XPos + 1, player.YPos, Direction.Right, _bulletSpeed, _bulletDamage, player.Name));
                                player.ReduceAmmo(1);

                            }
                            break;
                        case { Action: GameAction.Shoot, Direction: Direction.Down }:
                            if (PlayerPositionIsNotAtLowerBorder(player) &&
                                player.HasEnoughAmmo(1))
                            {
                                gameState.Bullets.Add(new BulletItem(player.XPos, player.YPos - 1, Direction.Down, _bulletSpeed, _bulletDamage, player.Name));
                                player.ReduceAmmo(1);

                            }
                            break;
                        case { Action: GameAction.Shoot, Direction: Direction.Left }:
                            if (PlayerPositionIsNotAtLeftBorder(player) &&
                                player.HasEnoughAmmo(1))
                            {
                                gameState.Bullets.Add(new BulletItem(player.XPos - 1, player.YPos, Direction.Left, _bulletSpeed, _bulletDamage, player.Name));
                                player.ReduceAmmo(1);
                            }
                            break;
                        case { Action: GameAction.Build, Direction: Direction.Up }:
                            if (PlayerPositionIsNotAtUpperBorder(player, gameState.MapYCellCount) &&
                                PositionIsNotOccupiedByObject(player.XPos, player.YPos + 1, gameState) &&
                                player.HasEnoughMaterials(1))
                            {
                                gameState.Walls.Add(new WallObject(player.XPos, player.YPos + 1, _wallHealth));
                                player.ReduceMaterials(1);

                            }
                            break;
                        case { Action: GameAction.Build, Direction: Direction.Right }:
                            if (PlayerPositionIsNotAtRightBorder(player, gameState.MapXCellCount) &&
                                PositionIsNotOccupiedByObject(player.XPos + 1, player.YPos, gameState) &&
                                player.HasEnoughMaterials(1))
                            {
                                gameState.Walls.Add(new WallObject(player.XPos + 1, player.YPos, _wallHealth));
                                player.ReduceMaterials(1);

                            }
                            break;
                        case { Action: GameAction.Build, Direction: Direction.Down }:
                            if (PlayerPositionIsNotAtLowerBorder(player) &&
                                PositionIsNotOccupiedByObject(player.XPos, player.YPos - 1, gameState) &&
                                player.HasEnoughMaterials(1))
                            {
                                gameState.Walls.Add(new WallObject(player.XPos, player.YPos - 1, _wallHealth));
                                player.ReduceMaterials(1);

                            }
                            break;
                        case { Action: GameAction.Build, Direction: Direction.Left }:
                            if (PlayerPositionIsNotAtLeftBorder(player) &&
                                PositionIsNotOccupiedByObject(player.XPos - 1, player.YPos, gameState) &&
                                player.HasEnoughMaterials(1))
                            {
                                gameState.Walls.Add(new WallObject(player.XPos - 1, player.YPos, _wallHealth));
                                player.ReduceMaterials(1);
                            }
                            break;
                        default:
                            break;
                    }

                    arePlayerActionsRemaining = true;
                }
            }

            actionCounter++;
        }
        while (arePlayerActionsRemaining && actionCounter <= _playerMaxSpeed);

        return Task.CompletedTask;
    }

    public bool PlayerPositionIsNotAtUpperBorder(IPlayer player, int mapYCellCount)
    {
        if (player.YPos < mapYCellCount - 1)
        {
            return true;
        }

        return false;
    }

    public bool PlayerPositionIsNotAtRightBorder(IPlayer player, int mapXCellCount)
    {
        if (player.XPos < mapXCellCount - 1)
        {
            return true;
        }

        return false;
    }

    public bool PlayerPositionIsNotAtLowerBorder(IPlayer player)
    {
        if (player.YPos > 0)
        {
            return true;
        }

        return false;
    }

    public bool PlayerPositionIsNotAtLeftBorder(IPlayer player)
    {
        if (player.XPos > 0)
        {
            return true;
        }

        return false;
    }

    public bool PositionIsNotOccupiedByObject(int x, int y, GameState gameState)
    {
        if (gameState.Map[y][x] is RenderId.Bomb or RenderId.Wall or RenderId.Player)
        {
            return false;
        }

        return true;
    }
}
