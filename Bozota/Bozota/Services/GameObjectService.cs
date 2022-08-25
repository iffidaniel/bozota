using Bozota.Models;

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
            // TODO
        }

        return Task.CompletedTask;
    }
}
