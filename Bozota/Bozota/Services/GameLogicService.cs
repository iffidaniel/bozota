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

    public Task<Player> AddNewPlayer(string name, int mapXCellCount, int mapYCellCount)
    {
        _logger.LogInformation("Adding new player: {player}", name);

        return Task.FromResult(new Player(name)
        {
            XPos = _random.Next(mapXCellCount), 
            YPos = _random.Next(mapYCellCount),
        });
    }

    public Task UpdatePlayerPositions(List<List<RenderId>> map, List<Player> players)
    {
        foreach (var player in players)
        {
            map[player.XPos][player.YPos] = RenderId.Player;
        }

        return Task.CompletedTask;
    }

    public RenderId GetRandomCellItem()
    {
        var randomNumber = _random.Next(100);
        if (randomNumber == 0 || randomNumber > 4)
        {
            return RenderId.Empty;
        }
        else if (randomNumber == 1)
        {
            return RenderId.Health;
        }
        else if (randomNumber == 2)
        {
            return RenderId.Ammo;
        }
        else if (randomNumber == 3)
        {
            return RenderId.Wall;
        }
        else if (randomNumber == 4)
        {
            return RenderId.Bomb;
        }
        else
        {
            return RenderId.Empty;
        }
    }
}
