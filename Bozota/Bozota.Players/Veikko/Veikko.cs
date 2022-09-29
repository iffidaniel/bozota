using Bozota.Common.Models;

namespace Bozota.Players.Veikko;

public class Veikko : IPlayingPlayer
{
    public string Name => "Veikko";

    public PlayerAction NextAction(GameState gameState, PlayerUtils playerUtils)
    {
        var action = new PlayerAction(Name, GameAction.Move, Direction.Left);

        return action;
    }
}
