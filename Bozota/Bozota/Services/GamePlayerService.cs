using Bozota.Models;
using Bozota.Models.Abstractions;
using Bozota.Models.Map;
using Bozota.Models.Map.Items;
using Bozota.Models.Map.Items.Abstractions;
using Bozota.Models.Map.Objects;

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

    public async Task<IPlayer?> AddNewPlayerWithRandomPosition(string name, GameState gameState)
    {
        _logger.LogInformation("Adding new player: {player}", name);

        IPlayer? player = null;

        // Check if randomly assigned position is already taken by other item or object on map
        var tryCounter = 0;
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

    public Task<IPlayer?> AddNewPlayer(string name, GameState gameState, int x, int y)
    {
        // Check if position is already occupied by other item or object on map
        var isPositionOccupiedByBombOrBombRadius = false;
        for (var bombX = 0 - _bombRadius; bombX <= _bombRadius; ++bombX)
        {
            for (var bombY = 0 - _bombRadius; bombY <= _bombRadius; ++bombY)
            {
                if (!(bombX == _bombRadius && bombY == _bombRadius) &&
                    !(bombX == _bombRadius && bombY == 0 - _bombRadius) &&
                    !(bombX == 0 - _bombRadius && bombY == _bombRadius) &&
                    !(bombX == 0 - _bombRadius && bombY == 0 - _bombRadius))
                {
                    if (IsPositionOccupied(x + bombX, y + bombY, gameState.Bombs))
                    {
                        isPositionOccupiedByBombOrBombRadius = true;
                        break;
                    }
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
            IsPositionOccupied(x, y, gameState.Players))
        {
            return Task.FromResult<IPlayer?>(null);
        }

        return Task.FromResult<IPlayer?>(new Player(name, x, y, _playerHealth, _playerMinHealth, _playerMaxHealth, _playerSpeed, _playerStartingAmmo, _playerStartingMaterials));
    }
    
    public static bool IsPositionOccupied<T>(int x, int y, List<T> items) where T : IMapItem
    {
        foreach (var item in items)
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
        List<IPlayer> deadPlayer = new();
        foreach (var player in gameState.Players)
        {
            if (!player.Health.IsAlive && !player.Health.IsInDestructable)
            {
                deadPlayer.Add(player);
            }
        }
        foreach (var player in deadPlayer)
        {
            gameState.Players.Remove(player);
        }

        // Process all player actions
        var actionCounter = 0;
        bool arePlayerActionsRemaining;
        do
        {
            arePlayerActionsRemaining = false;

            foreach (var player in gameState.Players)
            {
                if (player.Speed - actionCounter > 0)
                {
                    switch (player.Actions.Last())
                    {
                        case { Item1: PlayerAction.Move, Item2: Direction.Up }:
                            if (PlayerPositionIsNotAtUpperBorder(player, gameState.MapYCellCount) &&
                                PositionIsNotOccupiedByObject(player.XPos, player.YPos + 1, gameState))
                            {
                                player.YPos += 1;
                            }
                            break;
                        case { Item1: PlayerAction.Move, Item2: Direction.Right }:
                            if (PlayerPositionIsNotAtRightBorder(player, gameState.MapXCellCount) &&
                                PositionIsNotOccupiedByObject(player.XPos + 1, player.YPos, gameState))
                            {
                                player.XPos += 1;
                            }
                            break;
                        case { Item1: PlayerAction.Move, Item2: Direction.Down }:
                            if (PlayerPositionIsNotAtLowerBorder(player) &&
                                PositionIsNotOccupiedByObject(player.XPos, player.YPos - 1, gameState))
                            {
                                player.YPos -= 1;
                            }
                            break;
                        case { Item1: PlayerAction.Move, Item2: Direction.Left }:
                            if (PlayerPositionIsNotAtLeftBorder(player) &&
                                PositionIsNotOccupiedByObject(player.XPos - 1, player.YPos, gameState))
                            {
                                player.XPos -= 1;
                            }
                            break;
                        case { Item1: PlayerAction.Shoot, Item2: Direction.Up }:
                            if (PlayerPositionIsNotAtUpperBorder(player, gameState.MapYCellCount) &&
                                player.HasEnoughAmmo(1))
                            {
                                gameState.Bullets.Add(new BulletItem(player.XPos, player.YPos + 1, Direction.Up, _bulletSpeed, _bulletDamage));
                                player.ReduceAmmo(1);

                            }
                            break;
                        case { Item1: PlayerAction.Shoot, Item2: Direction.Right }:
                            if (PlayerPositionIsNotAtRightBorder(player, gameState.MapXCellCount) &&
                                player.HasEnoughAmmo(1))
                            {
                                gameState.Bullets.Add(new BulletItem(player.XPos + 1, player.YPos, Direction.Right, _bulletSpeed, _bulletDamage));
                                player.ReduceAmmo(1);

                            }
                            break;
                        case { Item1: PlayerAction.Shoot, Item2: Direction.Down }:
                            if (PlayerPositionIsNotAtLowerBorder(player) &&
                                player.HasEnoughAmmo(1))
                            {
                                gameState.Bullets.Add(new BulletItem(player.XPos, player.YPos - 1, Direction.Down, _bulletSpeed, _bulletDamage));
                                player.ReduceAmmo(1);

                            }
                            break;
                        case { Item1: PlayerAction.Shoot, Item2: Direction.Left }:
                            if (PlayerPositionIsNotAtLeftBorder(player) && 
                                player.HasEnoughAmmo(1))
                            {
                                gameState.Bullets.Add(new BulletItem(player.XPos - 1, player.YPos, Direction.Left, _bulletSpeed, _bulletDamage));
                                player.ReduceAmmo(1);
                            }
                            break;
                        case { Item1: PlayerAction.Build, Item2: Direction.Up }:
                            if (PlayerPositionIsNotAtUpperBorder(player, gameState.MapYCellCount) &&
                                player.HasEnoughMaterials(1))
                            {
                                gameState.Walls.Add(new WallObject(player.XPos, player.YPos + 1, _wallHealth));
                                player.ReduceMaterials(1);

                            }
                            break;
                        case { Item1: PlayerAction.Build, Item2: Direction.Right }:
                            if (PlayerPositionIsNotAtRightBorder(player, gameState.MapXCellCount) &&
                                player.HasEnoughMaterials(1))
                            {
                                gameState.Walls.Add(new WallObject(player.XPos + 1, player.YPos, _wallHealth));
                                player.ReduceMaterials(1);

                            }
                            break;
                        case { Item1: PlayerAction.Build, Item2: Direction.Down }:
                            if (PlayerPositionIsNotAtLowerBorder(player) &&
                                player.HasEnoughMaterials(1))
                            {
                                gameState.Walls.Add(new WallObject(player.XPos, player.YPos - 1, _wallHealth));
                                player.ReduceMaterials(1);

                            }
                            break;
                        case { Item1: PlayerAction.Build, Item2: Direction.Left }:
                            if (PlayerPositionIsNotAtLeftBorder(player) &&
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
        if (player.YPos > 1)
        {
            return true;
        }

        return false;
    }

    public bool PlayerPositionIsNotAtLeftBorder(IPlayer player)
    {
        if (player.XPos > 1)
        {
            return true;
        }

        return false;
    }

    public bool PositionIsNotOccupiedByObject(int x, int y, GameState gameState)
    {
        if (gameState.Map[x][y] is RenderId.Bomb or RenderId.Wall or RenderId.Player)
        {
            return false;
        }

        return true;
    }
}
