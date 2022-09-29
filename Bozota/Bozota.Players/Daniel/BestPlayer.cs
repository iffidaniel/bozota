using Bozota.Common.Models;

namespace Bozota.Players.Daniel;

public class BestPlayer : IPlayingPlayer
{
    public string Name => "Daniel";

    public PlayerAction NextAction(GameState gameState)
    {
        var action = GameAction.Move;
        var direction = Direction.Right;

        return new PlayerAction(Name, action, direction);
    }
}
