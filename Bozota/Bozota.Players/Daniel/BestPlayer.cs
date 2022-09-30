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
            var closestAmmo = gameStateUtils.FindClosestAmmoItem(new Position { X = player.XPos, Y = player.YPos });
            var closestPlayer = gameStateUtils.FindClosestPlayer(new Position { X = player.XPos, Y = player.YPos }, Name);
            var closestHealth = gameStateUtils.FindClosestHealthItem(new Position { X = player.XPos, Y = player.YPos });
            var closestMaterials = gameStateUtils.FindClosestMaterialsItem(new Position { X = player.XPos, Y = player.YPos });
        }

        var action = GameAction.Move;
        var direction = Direction.Right;

        return new PlayerAction(Name, action, direction);
    }
}
 