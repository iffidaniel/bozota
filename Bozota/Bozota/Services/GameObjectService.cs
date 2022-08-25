using Bozota.Models;
using Bozota.Models.Map.Objects.Abstractions;

namespace Bozota.Services;

public class GameObjectService
{
    private readonly ILogger<GameObjectService> _logger;

    public GameObjectService(ILogger<GameObjectService> logger, IConfiguration config)
    {
        _logger = logger;
    }

    public Task ProcessBombs(GameState gameState)
    {
        _logger.LogDebug("Processing bombs");

        foreach (var bomb in gameState.Bombs)
        {
            // TODO: Make bombs explode if it has no health or if player gets too close
        }

        return Task.CompletedTask;
    }

    public Task ProcessWalls(GameState gameState)
    {
        _logger.LogDebug("Processing walls");

        List<IWallObject> brokenWalls = new();
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
