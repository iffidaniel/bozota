using Bozota.Common.Models;
using Bozota.Common.Models.Players;
using System.Xml.Linq;

namespace Bozota.Players.Utils;

public static class ActionUtils
{
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
            int distance;
            if (p.YPos == me.YPos)
            {
                distance = Math.Abs(p.XPos - me.XPos);

                if (p.XPos > me.XPos)
                {
                    direction = Direction.Right;
                }
                if ((me.XPos - p.XPos) < closestDistance)
                {
                    direction = Direction.Left;
                }
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    action = new PlayerAction(shooterName, GameAction.Shoot, direction);
                }
            }
            else if (p.XPos == me.XPos)
            {
                distance = Math.Abs(p.YPos - me.YPos);

                if (p.YPos > me.YPos)
                {
                    direction = Direction.Up;
                }
                else
                {
                    direction = Direction.Down;
                }
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    action = new PlayerAction(shooterName, GameAction.Shoot, direction);
                }
            }
        }
        return action;
    }
}
