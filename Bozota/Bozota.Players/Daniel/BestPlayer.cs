using Bozota.Common.Models;
using Bozota.Players.Utils;

namespace Bozota.Players.Daniel;

public class BestPlayer : IPlayingPlayer
{
    public string Name => "Daniel";

    public PlayerAction NextAction(GameStateUtils gameStateUtils)
    {
        var player = gameStateUtils.GetPlayerStats(Name);

        if (player != null)
        {
            var closestAmmoItem = gameStateUtils.FindClosestAmmoItem(new Position { X = player.XPos, Y = player.YPos });
        }

        var action = GameAction.Move;
        var direction = Direction.Right;

        return new PlayerAction(Name, action, direction);
    }
}
 