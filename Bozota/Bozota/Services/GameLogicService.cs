using Bozota.Models;
using Bozota.Models.Abstractions;
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

    public Task<IPlayer> AddNewPlayer(string name, int mapXCellCount, int mapYCellCount)
    {
        _logger.LogInformation("Adding new player: {player}", name);

        return Task.FromResult<IPlayer>(new Player(name, _random.Next(mapXCellCount), _random.Next(mapYCellCount)));
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

    public PlayerMove GetRandomPlayerMove()
    {
        return (PlayerMove)_random.Next(5);
    }

    public Task MovePlayers(GameState gameState)
    {
        foreach (var player in gameState.Players)
        {
            if (!player.Health.IsAlive)
            {
                continue;
            }

            player.Moves.Add(GetRandomPlayerMove());

            if (player.Name == "Daniel")
            {
                _logger.LogDebug("player move: {move}", player.Moves.Last());
            }

            switch (player.Moves.Last())
            {
                case PlayerMove.Up:
                    if (player.YPos < gameState.MapYCellCount - 1 && gameState.Map[player.XPos][player.YPos + 1] != RenderId.Wall)
                    {
                        player.YPos += 1;
                    }
                    break;
                case PlayerMove.right:
                    if (player.XPos < gameState.MapXCellCount - 1 && gameState.Map[player.XPos + 1][player.YPos] != RenderId.Wall)
                    {
                        player.XPos += 1;
                    }
                    break;
                case PlayerMove.Down:
                    if (player.YPos > 1 && gameState.Map[player.XPos][player.YPos - 1] != RenderId.Wall)
                    {
                        player.YPos -= 1;
                    }
                    break;
                case PlayerMove.Left:
                    if (player.XPos > 1 && gameState.Map[player.XPos - 1][player.YPos] != RenderId.Wall)
                    {
                        player.XPos -= 1;
                    }
                    break;
                default:
                    break;
            }

            if (player.Name == "Daniel")
            {
                _logger.LogDebug("player pos: {x},{y}", player.XPos, player.YPos);
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
}
