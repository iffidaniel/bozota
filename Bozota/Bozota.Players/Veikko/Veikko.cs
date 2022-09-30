using Bozota.Common.Models;
using Bozota.Players.Utils;

namespace Bozota.Players.Veikko;

public class Veikko : IPlayingPlayer
{
    public string Name => "Veikko";

    public PlayerAction NextAction(GameStateUtils gameStateUtils)
    {
        var action = new PlayerAction(Name, GameAction.Move, Direction.Left);

        return action;
    }
}
