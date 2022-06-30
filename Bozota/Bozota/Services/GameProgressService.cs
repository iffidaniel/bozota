using Bozota.Models;

namespace Bozota.Services;

public class GameProgressService
{
    private readonly ILogger<GameProgressService> _logger;
    private readonly GameLogicService _gameLogic;
    private readonly GameMap _gameMap;
    private bool _isGameInitialized = false;

    public GameProgressService(ILogger<GameProgressService> logger, IConfiguration config,
        GameLogicService gameLogic)
    {
        _logger = logger;
        _gameLogic = gameLogic;

        _gameMap = new(config.GetValue("Game:MapXCellCount", 100), config.GetValue("Game:MapYCellCount", 100));
    }

    public Task InitializeGameAsync()
    {
        if (!_isGameInitialized)
        {
            _logger.LogInformation("Initializing game");

            for (int x = 0; x < _gameMap.XCellCount; x++)
            {
                var xMap = new List<CellItem>();
                for (int y = 0; y < _gameMap.XCellCount; y++)
                {
                    xMap.Add(CellItem.Empty);
                }
                _gameMap.Map.Add(xMap);
            }

            _gameMap.Players.Add(_gameLogic.AddNewPlayer("Veikko", _gameMap.XCellCount, _gameMap.YCellCount));
            _gameMap.Players.Add(_gameLogic.AddNewPlayer("Riku", _gameMap.XCellCount, _gameMap.YCellCount));
            _gameMap.Players.Add(_gameLogic.AddNewPlayer("Ramesh", _gameMap.XCellCount, _gameMap.YCellCount));
            _gameMap.Players.Add(_gameLogic.AddNewPlayer("Krishna", _gameMap.XCellCount, _gameMap.YCellCount));
            _gameMap.Players.Add(_gameLogic.AddNewPlayer("Raif", _gameMap.XCellCount, _gameMap.YCellCount));
            _gameMap.Players.Add(_gameLogic.AddNewPlayer("Daniel", _gameMap.XCellCount, _gameMap.YCellCount));
            _gameMap.Players.Add(_gameLogic.AddNewPlayer("Diana", _gameMap.XCellCount, _gameMap.YCellCount));
            _gameMap.Players.Add(_gameLogic.AddNewPlayer("Soikka", _gameMap.XCellCount, _gameMap.YCellCount));
            _gameMap.Players.Add(_gameLogic.AddNewPlayer("Hessu", _gameMap.XCellCount, _gameMap.YCellCount));
            _gameMap.Players.Add(_gameLogic.AddNewPlayer("Mika", _gameMap.XCellCount, _gameMap.YCellCount));

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

    public async Task UpdateGameProgress()
    {
        if (_isGameInitialized)
        {
            for (int x = 0; x < _gameMap.XCellCount; x++)
            {
                for (int y = 0; y < _gameMap.XCellCount; y++)
                {
                    _gameMap.Map[x][y] = _gameLogic.GetRandomCellItem();
                }
            }
        }

        await _gameLogic.UpdatePlayerPositions(_gameMap.Map, _gameMap.Players);
    }

    public bool IsGameInitialized() => _isGameInitialized;
}
