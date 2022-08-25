using Bozota.Models;

namespace Bozota.Services;

public class GameItemService
{
    private readonly ILogger<GameItemService> _logger;

    public GameItemService(ILogger<GameItemService> logger, IConfiguration config)
    {
        _logger = logger;
    }

    public Task ProcessBullets(GameState gameState)
    {
        _logger.LogDebug("Processing bullets");

        foreach (var bullet in gameState.Bullets)
        {
            // TODO
        }

        return Task.CompletedTask;
    }
}
