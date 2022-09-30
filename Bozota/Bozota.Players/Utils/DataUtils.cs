using Bozota.Common.Models;

namespace Bozota.Players.Utils;

public static class DataUtils
{
    public static int GetDistance(Position posA, Position posB)
    {
        var yDiff = posA.Y - posB.Y;
        var xDiff = posA.X - posB.X;

        return Math.Abs(yDiff) + Math.Abs(xDiff);
    }
}
