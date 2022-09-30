using Bozota.Common.Models;
using Bozota.Common.Models.Items;
using Bozota.Common.Models.Items.Abstractions;
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
            var closestAmmoItem1 = gameStateUtils.FindClosestAmmoItem(new Position { X = player.XPos, Y = player.YPos });
            //var closestAmmoItem2 = gameStateUtils.FindClosestItem<AmmoItem>(new Position { X = player.XPos, Y = player.YPos });
        }

        var action = GameAction.Move;
        var direction = Direction.Right;

        return new PlayerAction(Name, action, direction);
    }
}
 