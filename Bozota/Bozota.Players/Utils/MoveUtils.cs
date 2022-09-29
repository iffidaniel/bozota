using Bozota.Common.Models;

namespace Bozota.Players.Utils;

public static class MoveUtils
{
    //TODO: Doesn't take in account walls
    public static Direction MoveTowards(Position current, Position target)
    {
        var yDiff = current.Y - target.Y;
        var xDiff = current.X - target.X;

        if (Math.Abs(yDiff) < Math.Abs(xDiff))
        {
            //target is more to horizontal direction
            if (xDiff < 0)
            {
                return Direction.Right;
            }
            else
            {
                return Direction.Left;
            }
        }
        else if (Math.Abs(yDiff) >= Math.Abs(xDiff))
        {
            //target is more to vertical direction or diagonal direction
            if (yDiff < 0)
            {
                return Direction.Up;
            }
            else
            {
                return Direction.Down;
            }
        }

        return Direction.None;
    }
}