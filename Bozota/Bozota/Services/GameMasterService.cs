namespace Bozota.Services;

public class GameMasterService : IHostedService
{
    private readonly ILogger<GameTickerService> _logger;
    private readonly GameTickerService _gameTicker;

    public GameMasterService(ILogger<GameTickerService> logger, GameTickerService gameTicker)
    {
        _logger = logger;
        _gameTicker = gameTicker;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting {service}", nameof(GameMasterService));

        _gameTicker.StartGameTicker();

        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping {service}", nameof(GameMasterService));

        await _gameTicker.StopGameTicker();
    }
}
