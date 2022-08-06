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
        if (!_isGameInitialized)
        {
            _logger.LogInformation("Initializing game");

            for (int x = 0; x < _gameMap.XCellCount; x++)
            {
                var xMap = new List<CellItem>();
                for (int y = 0; y < _gameMap.XCellCount; y++)
                {
                    xMap.Add(_gameLogic.GetRandomCellItem());
                }
                _gameMap.Map.Add(xMap);
            }

            foreach (var name in _playerNames)
            {
                _gameMap.Players.Add(await _gameLogic.AddNewPlayer(name, _gameMap.XCellCount, _gameMap.YCellCount));
            }

            await _gameLogic.UpdatePlayerPositions(_gameMap.Map, _gameMap.Players);

            _logger.LogInformation("Game initialized");

            _isGameInitialized = true;

            return _gameMap;
        }

        return null;
    }

    public Task<GameMap?> GetGameProgress()
    {
        if (_isGameInitialized)
        {
            return Task.FromResult<GameMap?>(_gameMap);
        }

        return Task.FromResult<GameMap?>(null);
    }

    public async Task UpdateGameProgress()
    {
        if (_isGameInitialized)
        {
            var tempGameMap = _gameMap;

            for (int x = 0; x < _gameMap.XCellCount; x++)
            {
                for (int y = 0; y < _gameMap.XCellCount; y++)
                {
                    _gameMap.Map[x][y] = _gameLogic.GetRandomCellItem();
                }
            }

            await _gameLogic.UpdatePlayerPositions(_gameMap.Map, _gameMap.Players);

            lock (_gameMap)
            {
                _gameMap = tempGameMap;
            }
        }
    }

    public bool IsGameInitialized() => _isGameInitialized;
}
