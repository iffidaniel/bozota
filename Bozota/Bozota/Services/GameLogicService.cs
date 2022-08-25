using Bozota.Models;
using Bozota.Models.Map;

namespace Bozota.Services;

public class GameLogicService
{
    private readonly ILogger<GameLogicService> _logger;
    private readonly Random _random = new();

    public GameLogicService(ILogger<GameLogicService> logger, IConfiguration config)
    {
        _logger = logger;
    }

    public RenderId GetRandomMapItem(int seed)
    {
        return (RenderId)_random.Next(seed) switch
        {
            RenderId.Health => RenderId.Health,
            RenderId.Ammo => RenderId.Ammo,
            RenderId.Wall => RenderId.Wall,
            RenderId.Bomb => RenderId.Bomb,
            _ => RenderId.Empty,
        };
    }

    public Task RenderEmptyMap(GameState gameState)
    {
        for (int x = 0; x < gameState.MapXCellCount; x++)
        {
            for (int y = 0; y < gameState.MapXCellCount; y++)
            {
                gameState.Map[x][y] = RenderId.Empty;
            }
        }

        return Task.CompletedTask;
    }

    public Task RenderAllOnMap(GameState gameState)
    {
        foreach (var mapItem in gameState.Items)
        {
            gameState.Map[mapItem.XPos][mapItem.YPos] = mapItem.Render;
        }

        foreach (var mapObject in gameState.Objects)
        {
            gameState.Map[mapObject.XPos][mapObject.YPos] = mapObject.Render;
        }

        foreach (var player in gameState.Players)
        {
            gameState.Map[player.XPos][player.YPos] = player.Render;
        }

        return Task.CompletedTask;
    }

    public Task RemoveAllFromGame(GameState gameState)
    {
        _logger.LogInformation("Clearing all Players, Objects and Items from game");

        gameState.Items.Clear();
        gameState.Objects.Clear();
        gameState.Players.Clear();

        return Task.CompletedTask;
    }
}
