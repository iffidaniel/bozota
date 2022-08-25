using Bozota.Models;
using Bozota.Models.Abstractions;
using Bozota.Models.Map;

namespace Bozota.Services;

public class GamePlayerService
{
    private readonly ILogger<GamePlayerService> _logger;
    private readonly Random _random = new();

    public GamePlayerService(ILogger<GamePlayerService> logger, IConfiguration config)
    {
        _logger = logger;
    }

    public Task<IPlayer> AddNewPlayer(string name, GameState gameState)
    {
        _logger.LogInformation("Adding new player: {player}", name);

        int x;
        int y;
        bool cellTaken;
        do
        {
            x = _random.Next(gameState.MapXCellCount);
            y = _random.Next(gameState.MapYCellCount);
            cellTaken = false;

            foreach (var mapItem in gameState.Items)
            {
                if (mapItem.XPos == x && mapItem.YPos == y)
                {
                    cellTaken = true;
                    break;
                }
            }

            if (cellTaken)
            {
                continue;
            }

            foreach (var mapObject in gameState.Objects)
            {
                if (mapObject.XPos == x && mapObject.YPos == y)
                {
                    cellTaken = true;
                    break;
                }
            }

            if (cellTaken)
            {
                continue;
            }

            foreach (var player in gameState.Players)
            {
                if (player.XPos == x && player.YPos == y)
                {
                    cellTaken = true;
                    break;
                }
            }
        }
        while (cellTaken);

        return Task.FromResult<IPlayer>(new Player(name, x, y));
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
}
