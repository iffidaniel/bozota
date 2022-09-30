using Bozota.Common.Models;
using Bozota.Common.Models.Players;

namespace Bozota.Players.Utils;

public static class DataUtils
{
    public static int GetDistance(Position posA, Position posB)
    {
        var yDiff = posA.Y - posB.Y;
        var xDiff = posA.X - posB.X;

        return Math.Abs(yDiff) + Math.Abs(xDiff);
    }

    /// <summary>
    /// Calculates if there the player is on stright line vertically of horizontally
    /// </summary>
    /// <param name="from">Player to calculate from</param>
    /// <param name="to">Player to calculate to</param>
    /// <returns>Direction if player is on straigth line, Direction.None if none on the line</returns>
    public static Direction GetStraigthPlayerDirection(Player from, Player to)
    {
        if (from.YPos == to.YPos)
        {
            if (from.XPos - to.XPos < 0)
            {
                return Direction.Left;
            }
            else
            {
                return Direction.Right;
            }
        }
        else if (from.XPos == to.XPos)
        {
            if (from.YPos - to.YPos < 0)
            {
                return Direction.Down;
            }
            else
            {
                return Direction.Up;
            }
        }
        else
        {
            return Direction.None;
        }
    }
}
