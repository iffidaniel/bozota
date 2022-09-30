using Bozota.Common.Models;
using Bozota.Players.Utils;

namespace Bozota.Players.DummyPlayer;

public class DummyPlayer : IPlayingPlayer
{
    private readonly Random _random;

    public DummyPlayer(string name)
    {
        Name = name;
        _random = new Random();
    }

    public string Name { get; }

    public PlayerAction NextAction(GameStateUtils gameStateUtils)
    {
        return new PlayerAction(Name, (GameAction)_random.Next(4), (Direction)_random.Next(5));
    }
}
