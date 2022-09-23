using Bozota.Models;
using Bozota.Models.Map;
using Bozota.Models.Map.Items;
using Bozota.Models.Map.Objects;
using System;

namespace Bozota.Services;

public class GameMasterService
{
    private readonly ILogger<GameMasterService> _logger;
    private readonly Random _random = new();
    private readonly GameMapService _gameMapService;
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
        GameMapService gameMapService, GamePlayerService gamePlayerService, GameObjectService gameObjectService, GameItemService gameItemService)
    {
        _logger = logger;
        _gameMapService = gameMapService;
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

    public async Task InitializeGameAsync()
    {
        _logger.LogInformation("Initializing game");

        if (_isGameInitialized)
        {
            _logger.LogInformation("Game already initialized");
        }

        var tempState = _gameState;

        await _gameMapService.ClearAllFromGame(tempState);

        // Add random Objects and random Items and render empty map
        for (int y = 0; y < tempState.MapYCellCount; y++)
        {
            var row = new List<RenderId>();
            for (int x = 0; x < tempState.MapXCellCount; x++)
            {
                switch (_gameMapService.GetRandomMapItem(tempState.TotalCellCount / _randomGeneratorFrequency))
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
        await _gameMapService.RenderAllOnMap(tempState);

        lock (_gameState)
        {
            if (!_isGameInitialized)
            {
                _gameState = tempState;
                _isGameInitialized = true;
            }
        }

        _logger.LogInformation("Game initialized");
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

        // Add player actions
        foreach (var player in tempState.Players)
        {
            player.Actions.Add(new Tuple<PlayerAction, Direction>((PlayerAction)_random.Next(4), (Direction)_random.Next(5)));
        }

        // Process 
        await _gameItemService.ProcessAmmoItems(tempState);
        await _gameItemService.ProcessMaterialsItems(tempState);
        await _gameItemService.ProcessHealthItems(tempState);
        await _gameItemService.ProcessBullets(tempState);
        await _gameObjectService.ProcessBombs(tempState);
        await _gameItemService.ProcessFires(tempState);
        await _gameObjectService.ProcessWalls(tempState);

        await _gamePlayerService.ProcessPlayers(tempState);

        await _gameMapService.RenderEmptyMap(tempState);
        await _gameMapService.RenderAllOnMap(tempState);

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

    public async Task StopGameAsync()
    {
        _logger.LogInformation("Stopping game");

        if (!_isGameInitialized)
        {
            _logger.LogInformation("Game is not yet initialized");
        }

        var tempState = _gameState;

        await _gameMapService.ClearAllFromGame(tempState);

        lock (_gameState)
        {
            if (_isGameInitialized)
            {
                _gameState = tempState;
                _isGameInitialized = false;
            }
        }

        _logger.LogTrace("Game stopped");
    }
}
