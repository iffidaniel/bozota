using Bozota.Models;
using Bozota.Models.Map;
using Bozota.Models.Map.Items;
using Bozota.Models.Map.Objects;

namespace Bozota.Services;

public class GameMasterService
{
    private readonly ILogger<GameMasterService> _logger;
    private readonly GameMapService _gameLogic;
    private readonly GamePlayerService _gamePlayerService;
    private readonly GameItemService _gameItemService;
    private readonly GameObjectService _gameObjectService;
    private GameState _gameState;
    private readonly List<string> _playerNames;
    private readonly int _randomGeneratorFrequency;
    private readonly int _healAmount;
    private readonly int _ammoAmount;
    private readonly int _materialsAmount;
    private readonly int _wallHealth;
    private readonly int _bombHealth;
    private readonly int _bombDamage;
    private readonly int _bombRadius;
    private bool _isGameInitialized = false;
    private int updateCounter = 0;

    public GameMasterService(ILogger<GameMasterService> logger, IConfiguration config,
        GameMapService gameLogic, GamePlayerService gamePlayerService, GameObjectService gameObjectService, GameItemService gameItemService)
    {
        _logger = logger;
        _gameLogic = gameLogic;
        _gamePlayerService = gamePlayerService;
        _gameItemService = gameItemService;
        _gameObjectService = gameObjectService;

        _gameState = new(config.GetValue("Game:MapXCellCount", 100), config.GetValue("Game:MapYCellCount", 100));
        _playerNames = config.GetSection("Game:Players")?.GetChildren()?.Select(x => x.Value)?.ToList() ?? new List<string>();
        _randomGeneratorFrequency = config.GetValue("Game:RandomGeneratorFrequency", 40);
        _healAmount = config.GetValue("Game:HealAmount", 40);
        _ammoAmount = config.GetValue("Game:AmmoAmount", 10);
        _materialsAmount = config.GetValue("Game:MaterialsAmount", 5);
        _wallHealth = config.GetValue("Game:WallHealth", 80);
        _bombHealth = config.GetValue("Game:BombHealth", 80);
        _bombDamage = config.GetValue("Game:BombDamage", 80);
        _bombRadius = config.GetValue("Game:BombRadius", 3);
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

        // Add Fixed walls, random Objects, random Items and render empty map
        for (int x = 0; x < tempState.MapXCellCount; x++)
        {
            var row = new List<RenderId>();
            for (int y = 0; y < tempState.MapXCellCount; y++)
            {
                if (x == 0 || x == tempState.MapXCellCount - 1 || y == 0 || y == tempState.MapYCellCount - 1)
                {
                    tempState.Walls.Add(new WallObject(x, y, _wallHealth, true));
                }
                else
                {
                    switch (_gameLogic.GetRandomMapItem(tempState.TotalCellCount / _randomGeneratorFrequency))
                    {
                        case RenderId.Health:
                            tempState.HealthItems.Add(new HealthItem(x, y, _healAmount));
                            break;
                        case RenderId.Ammo:
                            tempState.AmmoItems.Add(new AmmoItem(x, y, _ammoAmount));
                            break;
                        case RenderId.Materials:
                            tempState.MaterialsItems.Add(new MaterialsItem(x, y, _materialsAmount));
                            break;
                        case RenderId.Wall:
                            tempState.Walls.Add(new WallObject(x, y, _wallHealth));
                            break;
                        case RenderId.Bomb:
                            tempState.Bombs.Add(new BombObject(x, y, _bombHealth, _bombDamage, _bombRadius));
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
            var newPlayer = await _gamePlayerService.AddNewPlayerWithRandomPosition(name, tempState);
            if (newPlayer != null)
            {
                tempState.Players.Add(newPlayer);
            }
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

        await _gameItemService.ProcessAmmoItems(tempState);
        await _gameItemService.ProcessMaterialsItems(tempState);
        await _gameItemService.ProcessHealthItems(tempState);
        await _gameItemService.ProcessBullets(tempState);
        await _gameObjectService.ProcessBombs(tempState);
        await _gameItemService.ProcessFires(tempState);
        await _gameObjectService.ProcessWalls(tempState);
        await _gamePlayerService.ProcessPlayers(tempState);
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
