namespace Bozota.Services;

public class GameTickerService : IAsyncDisposable
{
    private readonly ILogger<GameTickerService> _logger;
    private CancellationTokenSource _cts = new();
    private readonly int _minimumTickerIntervalInMiliseconds;
    private readonly int _maximumTickerIntervalInMiliseconds;
    private TimeSpan _tickerInterval;
    private PeriodicTimer _timer;
    private Task? _timerTask;
    private readonly GameProgressService _gameProgress;

    public GameTickerService(ILogger<GameTickerService> logger, IConfiguration config,
        GameProgressService gameProgress)
    {
        _logger = logger;

        _minimumTickerIntervalInMiliseconds = config.GetValue("Game:MinTickerInterval", 10);
        _maximumTickerIntervalInMiliseconds = config.GetValue("Game:MaxTickerInterval", 1000);
        var intervalInMilliseconds = config.GetValue("Game:TickerInterval", 500);

        if (intervalInMilliseconds <= _minimumTickerIntervalInMiliseconds)
        {
            _tickerInterval = new TimeSpan(0, 0, 0, 0, _minimumTickerIntervalInMiliseconds);
        }
        else if (intervalInMilliseconds >= _maximumTickerIntervalInMiliseconds)
        {
            _tickerInterval = new TimeSpan(0, 0, 0, 0, _maximumTickerIntervalInMiliseconds);
        }
        else
        {
            _tickerInterval = new TimeSpan(0, 0, 0, 0, intervalInMilliseconds);
        }

        _timer = new PeriodicTimer(_tickerInterval);

        _gameProgress = gameProgress;
    }

    public int GetTickerInterval()
    {
        return (int)_tickerInterval.TotalMilliseconds;
    }

    public async Task<int> SetTickerInterval(int intervalInMilliseconds)
    {
        if (GetTickerInterval() == intervalInMilliseconds)
        {
            return intervalInMilliseconds;
        }

        if (intervalInMilliseconds <= _minimumTickerIntervalInMiliseconds)
        {
            _tickerInterval = new TimeSpan(0, 0, 0, 0, _minimumTickerIntervalInMiliseconds);
        }
        else if (intervalInMilliseconds >= _maximumTickerIntervalInMiliseconds)
        {
            _tickerInterval = new TimeSpan(0, 0, 0, 0, _maximumTickerIntervalInMiliseconds);
        }
        else
        {
            _tickerInterval = new TimeSpan(0, 0, 0, 0, intervalInMilliseconds);
        }

        _logger.LogInformation("Game ticker interval set to {interval}ms", GetTickerInterval());

        if (_timerTask is not null)
        {
            await StopGameTicker();
            _timer.Dispose();
            _timer = new PeriodicTimer(_tickerInterval);
            StartGameTicker();
        }
        else
        {
            _timer = new PeriodicTimer(_tickerInterval);
        }

        return GetTickerInterval();
    }

    public void StartGameTicker()
    {
        _logger.LogInformation("Starting {service}", nameof(GameTickerService));

        if (_timerTask is null)
        {
            if (_cts.IsCancellationRequested)
            {
                _cts.Dispose();
                _cts = new();
            }

            _timerTask = DoWorkAsync();
        }

        _logger.LogInformation("Started {service}", nameof(GameTickerService));
    }

    public bool IsGameTickerRunning()
    {
        if (_timerTask is not null && _gameProgress.IsGameInitialized())
        {
            return true;
        }

        return false;
    }

    public async Task StopGameTicker()
    {
        _logger.LogInformation("Stopping {service}", nameof(GameTickerService));

        if (_timerTask is not null)
        {
            _cts.Cancel();
            await _timerTask;
            _timerTask = null;
        }

        _logger.LogInformation("Stopped {service}", nameof(GameTickerService));
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
