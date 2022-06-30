using Bozota.Models;

namespace Bozota.Services;

public class GameLogicService
{
    private readonly ILogger<GameLogicService> _logger;
    private readonly Random _random = new();

    public GameLogicService(ILogger<GameLogicService> logger, IConfiguration config)
    {
        _logger = logger;
    }

    public Player AddNewPlayer(string name, int mapXCellCount, int mapYCellCount)
    {
        _logger.LogInformation("Adding new player: {player}", name);

        return new Player
        {
            Name = name,
            xPos = name == "Daniel" ? 0 : _random.Next(mapXCellCount), // TODO: Hard coded Daniel's X position for testing purposes
            yPos = name == "Daniel" ? 1 : _random.Next(mapYCellCount), // TODO: Hard coded Daniel's Y position for testing purposes
        };
    }

    public Task UpdatePlayerPositions(List<List<CellItem>> map, List<Player> players)
    {
        foreach (var player in players)
        {
            map[player.xPos][player.yPos] = CellItem.Player;
        }

        return Task.CompletedTask;
    }

    public CellItem GetRandomCellItem()
    {
        var randomNumber = _random.Next(100);
        if (randomNumber == 0 || randomNumber > 4)
        {
            return CellItem.Empty;
        }
        else if (randomNumber == 1)
        {
            return CellItem.Health;
        }
        else if (randomNumber == 2)
        {
            return CellItem.Ammo;
        }
        else if (randomNumber == 3)
        {
            return CellItem.Wall;
        }
        else if (randomNumber == 4)
        {
            return CellItem.Bomb;
        }
        else
        {
            return CellItem.Empty;
        }
    }
}
