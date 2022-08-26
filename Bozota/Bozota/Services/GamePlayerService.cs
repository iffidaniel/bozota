﻿using Bozota.Models;
using Bozota.Models.Abstractions;
using Bozota.Models.Map;
using Bozota.Models.Map.Items;
using Bozota.Models.Map.Items.Abstractions;

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
    private readonly int _bulletSpeed;
    private readonly int _bulletDamage;
    private readonly int _bombRadius;

    public GamePlayerService(ILogger<GamePlayerService> logger, IConfiguration config)
    {
        _logger = logger;

        _playerHealth = config.GetValue("Game:PlayerHealth", 100);
        _playerMinHealth = config.GetValue("Game:PlayerMinHealth", 10);
        _playerMaxHealth = config.GetValue("Game:PlayerMaxHealth", 200);
        _playerSpeed = config.GetValue("Game:PlayerSpeed", 1);
        _playerMaxSpeed = config.GetValue("Game:PlayerMaxSpeed", 5);
        _playerStartingAmmo = config.GetValue("Game:PlayerStartingAmmo", 10);
        _bulletSpeed = config.GetValue("Game:BulletSpeed", 3);
        _bulletDamage = config.GetValue("Game:BulletDamage", 40);
        _bombRadius = config.GetValue("Game:BombRadius", 3);
    }

    public Task<IPlayer> AddNewPlayer(string name, GameState gameState)
    {
        _logger.LogInformation("Adding new player: {player}", name);

        var x = 0;
        var y = 0;
        var tryCounter = 0;
        while (tryCounter <= gameState.TotalCellCount)
        {
            x = _random.Next(gameState.MapXCellCount);
            y = _random.Next(gameState.MapYCellCount);
            tryCounter++;

            if (IsMapCellTaken(x, y, gameState.AmmoItems)) { continue; }
            if (IsMapCellTaken(x, y, gameState.HealthItems)) { continue; }
            var mapCellTaken = false;
            for (var bombX = 0 - _bombRadius; bombX <= _bombRadius; ++bombX)
            {
                for (var bombY = 0 - _bombRadius; bombY <= _bombRadius; ++bombY)
                {
                    if (!(bombX == _bombRadius && bombY == _bombRadius) &&
                        !(bombX == _bombRadius && bombY == 0 - _bombRadius) &&
                        !(bombX == 0 - _bombRadius && bombY == _bombRadius) &&
                        !(bombX == 0 - _bombRadius && bombY == 0 - _bombRadius))
                    {
                        if (IsMapCellTaken(x + bombX, y + bombY, gameState.Bombs))
                        {
                            mapCellTaken = true;
                            break;
                        }
                    }
                }
                if (mapCellTaken) { break; }
            }
            if (mapCellTaken) { continue; }
            if (IsMapCellTaken(x, y, gameState.Walls)) { continue; }
            if (IsMapCellTaken(x, y, gameState.Players)) { continue; }

            break;
        }

        return Task.FromResult<IPlayer>(new Player(name, x, y, _playerHealth, _playerMinHealth, _playerMaxHealth, _playerSpeed, _playerStartingAmmo));
    }

    public bool IsMapCellTaken<T>(int x, int y, List<T> items) where T : IMapItem
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

    public Tuple<PlayerAction, Direction> GetRandomPlayerAction()
    {
        return new Tuple<PlayerAction, Direction>((PlayerAction)_random.Next(3), (Direction)_random.Next(5));
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

        var actionCounter = 0;
        bool arePlayerActionsRemaining;
        do
        {
            arePlayerActionsRemaining = false;

            foreach (var player in gameState.Players)
            {
                if (player.Speed - actionCounter > 0)
                {
                    // TODO: This randomly added player action should be replaced with actual action from player
                    player.Actions.Add(GetRandomPlayerAction());

                    switch (player.Actions.Last())
                    {
                        case { Item1: PlayerAction.Move, Item2: Direction.Up }:
                            if (player.YPos < gameState.MapYCellCount - 1 &&
                                gameState.Map[player.XPos][player.YPos + 1] != RenderId.Wall &&
                                gameState.Map[player.XPos][player.YPos + 1] != RenderId.Player)
                            {
                                player.YPos += 1;
                            }
                            break;
                        case { Item1: PlayerAction.Move, Item2: Direction.Right }:
                            if (player.XPos < gameState.MapXCellCount - 1 &&
                                gameState.Map[player.XPos + 1][player.YPos] != RenderId.Wall &&
                                gameState.Map[player.XPos + 1][player.YPos] != RenderId.Player)
                            {
                                player.XPos += 1;
                            }
                            break;
                        case { Item1: PlayerAction.Move, Item2: Direction.Down }:
                            if (player.YPos > 1 &&
                                gameState.Map[player.XPos][player.YPos - 1] != RenderId.Wall &&
                                gameState.Map[player.XPos][player.YPos - 1] != RenderId.Player)
                            {
                                player.YPos -= 1;
                            }
                            break;
                        case { Item1: PlayerAction.Move, Item2: Direction.Left }:
                            if (player.XPos > 1 &&
                                gameState.Map[player.XPos - 1][player.YPos] != RenderId.Wall &&
                                gameState.Map[player.XPos - 1][player.YPos] != RenderId.Player)
                            {
                                player.XPos -= 1;
                            }
                            break;
                        case { Item1: PlayerAction.Shoot, Item2: Direction.Up }:
                            if (player.Ammo > 0 && player.YPos < gameState.MapYCellCount - 1)
                            {
                                gameState.Bullets.Add(new BulletItem(player.XPos, player.YPos + 1, Direction.Up, _bulletSpeed, _bulletDamage));
                                player.Ammo -= 1;

                            }
                            break;
                        case { Item1: PlayerAction.Shoot, Item2: Direction.Right }:
                            if (player.Ammo > 0 && player.XPos < gameState.MapXCellCount - 1)
                            {
                                gameState.Bullets.Add(new BulletItem(player.XPos + 1, player.YPos, Direction.Right, _bulletSpeed, _bulletDamage));
                                player.Ammo -= 1;

                            }
                            break;
                        case { Item1: PlayerAction.Shoot, Item2: Direction.Down }:
                            if (player.Ammo > 0 && player.YPos > 1)
                            {
                                gameState.Bullets.Add(new BulletItem(player.XPos, player.YPos - 1, Direction.Down, _bulletSpeed, _bulletDamage));
                                player.Ammo -= 1;

                            }
                            break;
                        case { Item1: PlayerAction.Shoot, Item2: Direction.Left }:
                            if (player.Ammo > 0 && player.XPos > 1)
                            {
                                gameState.Bullets.Add(new BulletItem(player.XPos - 1, player.YPos, Direction.Left, _bulletSpeed, _bulletDamage));
                                player.Ammo -= 1;
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
}
