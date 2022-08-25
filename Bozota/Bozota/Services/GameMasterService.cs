using Bozota.Models;
using Bozota.Models.Map;
using Bozota.Models.Map.Items;
using Bozota.Models.Map.Objects;

namespace Bozota.Services;

public class GameMasterService
{
    private readonly ILogger<GameMasterService> _logger;
    private readonly GameLogicService _gameLogic;
    private readonly GamePlayerService _gamePlayerService;
    private GameState _gameState;
    private readonly List<string> _playerNames;
    private bool _isGameInitialized = false;
    private int updateCounter = 0;

    public GameMasterService(ILogger<GameMasterService> logger, IConfiguration config,
        GameLogicService gameLogic, GamePlayerService gamePlayerService)
    {
        _logger = logger;
        _gameLogic = gameLogic;
        _gamePlayerService = gamePlayerService;

        _gameState = new(config.GetValue("Game:MapXCellCount", 100), config.GetValue("Game:MapYCellCount", 100));
        _playerNames = config.GetSection("Game:Players")?.GetChildren()?.Select(x => x.Value)?.ToList() ?? new List<string>();
    }

    public bool IsGameInitialized() => _isGameInitialized;

    public async Task<GameState?> InitializeGameAsync()
    {
        _logger.LogInformation("Initializing game");

        if (_isGameInitialized)
        {
            _logger.LogWarning("Game already initialized");

            return null;
        }

        var tempState = _gameState;

        // Add Fixed walls, random Objects and Items and render empty map
        for (int x = 0; x < tempState.MapXCellCount; x++)
        {
            var row = new List<RenderId>();
            for (int y = 0; y < tempState.MapXCellCount; y++)
            {
                if (x == 0 || x == tempState.MapXCellCount - 1 || y == 0 || y == tempState.MapYCellCount - 1)
                {
                    tempState.Objects.Add(new Wall(x, y, true));
                }
                else
                {
                    switch (_gameLogic.GetRandomMapItem(tempState.MapXCellCount * tempState.MapXCellCount / 4))
                    {
                        case RenderId.Health:
                            tempState.Items.Add(new HealthItem(x, y, 40));
                            break;
                        case RenderId.Ammo:
                            tempState.Items.Add(new AmmoItem(x, y, 10));
                            break;
                        case RenderId.Wall:
                            tempState.Objects.Add(new Wall(x, y));
                            break;
                        case RenderId.Bomb:
                            tempState.Objects.Add(new Bomb(x, y, 80, 2));
                            break;
                    };
                }

                row.Add(RenderId.Empty);
            }
            tempState.Map.Add(row);
        }

        // Add Players
        foreach (var name in _playerNames)
        {
            var newPlayer = await _gamePlayerService.AddNewPlayer(name, tempState);
            tempState.Players.Add(newPlayer);
        }

        // Render Players, Object and Items on map
        await _gameLogic.RenderAllOnMap(tempState);

        lock (_gameState)
        {
            if (!_isGameInitialized)
            {
                _gameState = tempState;
                _isGameInitialized = true;
            }
        }

        _logger.LogInformation("Game initialized");

        return _gameState;
    }

    public async Task<GameState?> UpdateGameAsync()
    {
        updateCounter++;
        _logger.LogInformation("Updating game progress, {count}", updateCounter);

        if (!_isGameInitialized)
        {
            _logger.LogWarning("Game is not yet initialized");

            return null;
        }

        var tempState = _gameState;

        await _gamePlayerService.MovePlayers(tempState);
        await _gameLogic.RenderEmptyMap(tempState);
        await _gameLogic.RenderAllOnMap(tempState);

        lock (_gameState)
        {
            if (_isGameInitialized)
            {
                _gameState = tempState;
            }
        }

        _logger.LogTrace("Game updated");

        return _gameState;
    }

    public async Task<GameState?> StopGameAsync()
    {
        _logger.LogInformation("Stopping game");

        if (!_isGameInitialized)
        {
            _logger.LogWarning("Game is not yet initialized");

            return null;
        }

        var tempState = _gameState;

        await _gameLogic.RemoveAllFromGame(tempState);
        await _gameLogic.RenderEmptyMap(tempState);

        lock (_gameState)
        {
            if (_isGameInitialized)
            {
                _gameState = tempState;
                _isGameInitialized = false;
            }
        }

        _logger.LogTrace("Game stopped");

        return _gameState;
    }
}
