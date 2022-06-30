using Bozota.Models;

namespace Bozota.Services;

public class GameProgressService
{
    private readonly ILogger<GameProgressService> _logger;
    private readonly int _mapXCellCount;
    private readonly int _mapYCellCount;
    private readonly GameMap _gameMap;
    private bool _isGameInitialized = false;
    private readonly Random _random = new();

    public GameProgressService(ILogger<GameProgressService> logger, IConfiguration config)
    {
        _logger = logger;

        _mapXCellCount = config.GetValue("Game:MapXCellCount", 100);
        _mapYCellCount = config.GetValue("Game:MapYCellCount", 100);

        _gameMap = new(_mapXCellCount, _mapYCellCount);
    }

    public Task InitializeGameAsync()
    {
        if (!_isGameInitialized)
        {
            _logger.LogInformation("Initializing game");

            for (int x = 0; x < _mapXCellCount; x++)
            {
                var xMap = new List<CellItem>();
                for (int y = 0; y < _mapXCellCount; y++)
                {
                    xMap.Add(CellItem.Empty);
                }
                _gameMap.Map.Add(xMap);
            }

            _isGameInitialized = true;
        }

        return Task.CompletedTask;
    }

    public Task<GameMap?> GetGameProgress()
    {
        if (_isGameInitialized)
        {
            return Task.FromResult<GameMap?>(_gameMap);
        }

        return Task.FromResult<GameMap?>(null);
    }

    public Task UpdateGameProgress()
    {
        if (_isGameInitialized)
        {
            for (int x = 0; x < _mapXCellCount; x++)
            {
                for (int y = 0; y < _mapXCellCount; y++)
                {
                    _gameMap.Map[x][y] = GetRandomCellItem();
                }
            }
        }

        return Task.CompletedTask;
    }

    public bool IsGameInitialized() => _isGameInitialized;

    private CellItem GetRandomCellItem()
    {
        var randomNumber = _random.Next(100);
        if (randomNumber == 0 || randomNumber > 4)
        {
            return CellItem.Empty;
        }
        else if (randomNumber == 1)
        {
            return CellItem.Health;
        }
        else if (randomNumber == 2)
        {
            return CellItem.Ammo;
        }
        else if (randomNumber == 3)
        {
            return CellItem.Wall;
        }
        else if (randomNumber == 4)
        {
            return CellItem.Bomb;
        }
        else
        {
            return CellItem.Empty;
        }
    }
}
