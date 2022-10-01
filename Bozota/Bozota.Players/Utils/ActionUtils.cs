using Bozota.Common.Models;
using Bozota.Common.Models.Players;
using System.Xml.Linq;

namespace Bozota.Players.Utils;

public static class ActionUtils
{
    /// <summary>
    /// Shoot the closest if the player is in x or y axis sector
    /// </summary>
    public static PlayerAction? ShootClosest(string shooterName, List<Player> players)
    {
        var me = players.First(p => p.Name == shooterName);
        var otherplayers = new List<Player>(players);
        otherplayers.Remove(me);

        int closestDistance = 1000;
        PlayerAction? action = null;
        Direction direction = Direction.None;
        foreach (var p in otherplayers)
        {
            int distance = int.MaxValue;
            if (p.YPos == me.YPos)
            {
                distance = Math.Abs(p.XPos - me.XPos);
                direction = p.XPos > me.XPos ? Direction.Right : Direction.Left;
            }
            else if (p.XPos == me.XPos)
            {
                distance = Math.Abs(p.YPos - me.YPos);
                direction = p.YPos > me.YPos ? Direction.Up : Direction.Down;
            }
            if (distance < closestDistance)
            {
                closestDistance = distance;
                action = new PlayerAction(shooterName, GameAction.Shoot, direction);
            }
        }
        return action;
    }
}
