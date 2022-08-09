using Bozota.Models;

namespace Bozota.Services;

public class GameProgressService
{
    private readonly ILogger<GameProgressService> _logger;
    private readonly GameLogicService _gameLogic;
    private GameMap _gameMap;
    private readonly List<string> _playerNames;
    private bool _isGameInitialized = false;

    public GameProgressService(ILogger<GameProgressService> logger, IConfiguration config,
        GameLogicService gameLogic)
    {
        _logger = logger;
        _gameLogic = gameLogic;

        _gameMap = new(config.GetValue("Game:MapXCellCount", 100), config.GetValue("Game:MapYCellCount", 100));
        _playerNames = config.GetSection("Game:Players")?.GetChildren()?.Select(x => x.Value)?.ToList() ?? new List<string>();
    }

    public async Task<GameMap?> InitializeGameAsync()
    {
        _logger.LogInformation("Initializing game");

        if (_isGameInitialized)
        {
            _logger.LogWarning("Game already initialized");

            return null;
        }

        var tempGameMap = _gameMap;

        for (int x = 0; x < tempGameMap.XCellCount; x++)
        {
            var xMap = new List<CellItem>();
            for (int y = 0; y < tempGameMap.XCellCount; y++)
            {
                xMap.Add(_gameLogic.GetRandomCellItem());
            }
            tempGameMap.Map.Add(xMap);
        }

        foreach (var name in _playerNames)
        {
            tempGameMap.Players.Add(await _gameLogic.AddNewPlayer(name, tempGameMap.XCellCount, tempGameMap.YCellCount));
        }

        await _gameLogic.UpdatePlayerPositions(tempGameMap.Map, tempGameMap.Players);

        lock (_gameMap)
        {
            _gameMap = tempGameMap;
        }

        _logger.LogInformation("Game initialized");

        _isGameInitialized = true;

        return _gameMap;
    }

    public async Task<GameMap?> UpdateGameProgressAsync()
    {
        _logger.LogTrace("Updating game progress");

        if (!_isGameInitialized)
        {
            _logger.LogWarning("Game already initialized");

            return null;
        }

        var tempGameMap = _gameMap;

        for (int x = 0; x < tempGameMap.XCellCount; x++)
        {
            for (int y = 0; y < tempGameMap.XCellCount; y++)
            {
                tempGameMap.Map[x][y] = _gameLogic.GetRandomCellItem();
            }
        }

        await _gameLogic.UpdatePlayerPositions(tempGameMap.Map, tempGameMap.Players);

        lock (_gameMap)
        {
            _gameMap = tempGameMap;
        }

        _logger.LogTrace("Game progress updated");

        return _gameMap;
    }

    public bool IsGameInitialized() => _isGameInitialized;
}
