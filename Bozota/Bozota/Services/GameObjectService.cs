using Bozota.Models;
using Bozota.Models.Map.Items;
using Bozota.Models.Map.Objects;

namespace Bozota.Services;

public class GameObjectService
{
    private readonly ILogger<GameObjectService> _logger;
    private readonly int _fireDuration;

    public GameObjectService(ILogger<GameObjectService> logger, IConfiguration config)
    {
        _logger = logger;

        _fireDuration = config.GetValue("Game:FireDuration", 3);
    }

    public Task ProcessBombs(GameState gameState)
    {
        _logger.LogDebug("Processing bombs");

        List<BombObject> explodedBombs = new();
        foreach (var bomb in gameState.Bombs)
        {
            if (!bomb.Health.IsAlive && !bomb.Health.IsInDestructable)
            {
                explodedBombs.Add(bomb);
                continue;
            }

            foreach (var player in gameState.Players)
            {
                if (player.XPos >= bomb.XPos - bomb.ExplosionRadius &&
                    player.XPos <= bomb.XPos + bomb.ExplosionRadius &&
                    player.YPos >= bomb.YPos - bomb.ExplosionRadius &&
                    player.YPos <= bomb.YPos + bomb.ExplosionRadius)
                {
                    explodedBombs.Add(bomb);
                    break;
                }
            }
        }

        // Create explosions and remove exploded bombs
        foreach (var bomb in explodedBombs)
        {
            for (var y = 0 - bomb.ExplosionRadius; y <= bomb.ExplosionRadius; y++)
            {
                for (var x = 0 - bomb.ExplosionRadius; x <= bomb.ExplosionRadius; x++)
                {
                    if (bomb.XPos + x >= 0 && bomb.XPos + x < gameState.MapXCellCount &&
                        bomb.YPos + y >= 0 && bomb.YPos + y < gameState.MapYCellCount &&
                        !(x == bomb.ExplosionRadius && y == bomb.ExplosionRadius) &&
                        !(x == bomb.ExplosionRadius && y == 0 - bomb.ExplosionRadius) &&
                        !(x == 0 - bomb.ExplosionRadius && y == bomb.ExplosionRadius) &&
                        !(x == 0 - bomb.ExplosionRadius && y == 0 - bomb.ExplosionRadius))
                    {
                        gameState.FireItems.Add(new FireItem(bomb.XPos + x, bomb.YPos + y, _fireDuration, bomb.ExplosionDamage));
                    }
                }
            }

            gameState.Bombs.Remove(bomb);
        }

        return Task.CompletedTask;
    }

    public Task ProcessWalls(GameState gameState)
    {
        _logger.LogDebug("Processing walls");

        List<WallObject> brokenWalls = new();
        foreach (var wall in gameState.Walls)
        {
            if (!wall.Health.IsAlive && !wall.Health.IsInDestructable)
            {
                brokenWalls.Add(wall);
            }
        }

        // Remove broken walls
        foreach (var wall in brokenWalls)
        {
            gameState.Walls.Remove(wall);
        }

        return Task.CompletedTask;
    }
}
