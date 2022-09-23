using Bozota.Models;
using Bozota.Models.Map;
using Bozota.Models.Map.Items.Abstractions;

namespace Bozota.Services;

public class GameMapService
{
    private readonly ILogger<GameMapService> _logger;
    private readonly Random _random = new();

    public GameMapService(ILogger<GameMapService> logger, IConfiguration config)
    {
        _logger = logger;
    }

    public RenderId GetRandomMapItem(int seed)
    {
        return (RenderId)_random.Next(seed) switch
        {
            RenderId.Health => RenderId.Health,
            RenderId.Ammo => RenderId.Ammo,
            RenderId.Materials => RenderId.Materials,
            RenderId.Wall => RenderId.Wall,
            RenderId.Bomb => RenderId.Bomb,
            _ => RenderId.Empty,
        };
    }

    public Task RenderEmptyMap(GameState gameState)
    {
        for (int y = 0; y < gameState.MapYCellCount; y++)
        {
            for (int x = 0; x < gameState.MapXCellCount; x++)
            {
                gameState.Map[y][x] = RenderId.Empty;
            }
        }

        return Task.CompletedTask;
    }

    public Task RenderAllOnMap(GameState gameState)
    {
        RenderOnMap(gameState.Map, gameState.AmmoItems);
        RenderOnMap(gameState.Map, gameState.MaterialsItems);
        RenderOnMap(gameState.Map, gameState.HealthItems);
        RenderOnMap(gameState.Map, gameState.FireItems);
        RenderOnMap(gameState.Map, gameState.Bullets);
        RenderOnMap(gameState.Map, gameState.Bombs);
        RenderOnMap(gameState.Map, gameState.Walls);
        RenderOnMap(gameState.Map, gameState.Players);

        return Task.CompletedTask;
    }

    public void RenderOnMap<T>(List<List<RenderId>> map, List<T> items) where T : IMapItem
    {
        foreach (var item in items)
        {
            map[item.YPos][item.XPos] = item.Render;
        }
    }

    public Task ClearAllFromGame(GameState gameState)
    {
        _logger.LogInformation("Clearing all Players, Objects and Items from game");

        gameState.AmmoItems.Clear();
        gameState.MaterialsItems.Clear();
        gameState.HealthItems.Clear();
        gameState.FireItems.Clear();
        gameState.Bullets.Clear();
        gameState.Bombs.Clear();
        gameState.Walls.Clear();
        gameState.Players.Clear();
        ClearMap(gameState);

        return Task.CompletedTask;
    }

    public void ClearMap(GameState gameState)
    {
        foreach (var row in gameState.Map)
        {
            row.Clear();
        }
        gameState.Map.Clear();
    }
}
