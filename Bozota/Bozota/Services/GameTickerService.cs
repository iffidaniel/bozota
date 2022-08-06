using Bozota.Models;

namespace Bozota.Services;

public class GameTickerService : IAsyncDisposable
{
    private readonly ILogger<GameTickerService> _logger;
    private readonly GameTicker _gameTicker;
    private CancellationTokenSource _cts = new();
    private PeriodicTimer _timer;
    private Task? _timerTask;
    private readonly GameProgressService _gameProgress;

    public GameTickerService(ILogger<GameTickerService> logger, IConfiguration config,
        GameProgressService gameProgress)
    {
        _logger = logger;

        _gameTicker = new(config.GetValue("Game:MinTickerInterval", 10), config.GetValue("Game:MaxTickerInterval", 2000))
        {
            Interval = config.GetValue("Game:TickerInterval", 500)
        };

        _timer = new PeriodicTimer(new TimeSpan(0, 0, 0, 0, _gameTicker.Interval));

        _gameProgress = gameProgress;
    }

    public Task<GameTicker> GetTicker()
    {
        _gameTicker.IsRunning = IsGameTickerRunning();

        return Task.FromResult(_gameTicker);
    }

    public async Task<GameTicker> SetTickerInterval(int intervalInMilliseconds)
    {
        if (_gameTicker.Interval == intervalInMilliseconds)
        {
            return _gameTicker;
        }
        _gameTicker.Interval = intervalInMilliseconds;

        _logger.LogInformation("Game ticker interval set to {interval}ms", _gameTicker.Interval);

        if (_timerTask is not null)
        {
            await StopGameTicker();
            _timer.Dispose();
            _timer = new PeriodicTimer(new TimeSpan(0, 0, 0, 0, _gameTicker.Interval));
            await StartGameTicker();
        }
        else
        {
            _timer = new PeriodicTimer(new TimeSpan(0, 0, 0, 0, _gameTicker.Interval));
        }

        _gameTicker.IsRunning = IsGameTickerRunning();

        return _gameTicker;
    }

    public Task<GameTicker> StartGameTicker()
    {
        if (_timerTask is null)
        {
            if (_cts.IsCancellationRequested)
            {
                _cts.Dispose();
                _cts = new();
            }

            _timerTask = DoWorkAsync();
        }

        _gameTicker.IsRunning = IsGameTickerRunning();
        
        _logger.LogInformation("Game ticker started");

        return Task.FromResult(_gameTicker);
    }

    public async Task<GameTicker> StopGameTicker()
    {
        if (_timerTask is not null)
        {
            _cts.Cancel();
            await _timerTask;
            _timerTask = null;
        }

        _gameTicker.IsRunning = IsGameTickerRunning();

        _logger.LogInformation("Game ticker stopped");

        return _gameTicker;
    }

    public async ValueTask DisposeAsync()
    {
        _logger.LogInformation("Disposing {service}", nameof(GameTickerService));

        GC.SuppressFinalize(this);

        if (_timerTask is not null)
        {
            _cts.Cancel();
            await _timerTask;
        }
        _cts.Dispose();
        _timer?.Dispose();

        _logger.LogInformation("Disposed {service}", nameof(GameTickerService));
    }

    private bool IsGameTickerRunning()
    {
        if (_timerTask is not null && _gameProgress.IsGameInitialized())
        {
            return true;
        }

        return false;
    }

    private async Task DoWorkAsync()
    {
        try
        {
            while (await _timer.WaitForNextTickAsync(_cts.Token))
            {
                if (_gameProgress.IsGameInitialized())
                {
                    _logger.LogInformation("Updating game map at time {time}", DateTime.Now.ToString("O"));
                    await _gameProgress.UpdateGameProgress();
                }
            }
        }
        catch (OperationCanceledException)
        {
        }
    }
}
