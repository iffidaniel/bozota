using Bozota.Common.Models;

namespace Bozota.Players.Utils;

public static class MoveUtils
{
    private readonly static Direction[] everyDirection = { Direction.Down, Direction.Left, Direction.Right, Direction.Up };

    /// <summary>
    /// Rules out impassable directions and decides best direction towards certain coordinates
    /// </summary>
    /// <param name="current">where robot is stationed now</param>
    /// <param name="target">where robot needs to ge to</param>
    /// <param name="impassable">impassable positions like walls</param>
    /// <returns>best direction to go on next turn</returns>
    public static Direction MoveTowards(Position current, Position target, List<Position> impassable)
    {
        var disallowed = new List<Direction>();

        // calculate disallowed
        foreach (var dir in everyDirection)
        {
            var newPos = PositionAfterMove(current, dir);
            if (impassable.Contains(newPos))
            {
                disallowed.Add(dir);
            }
        }

        var preferredNextDirection = BestDirection(current, target, disallowed);

        return preferredNextDirection;
    }

    private static Direction BestDirection(Position current, Position target, List<Direction> disallowed)
    {
        var yDiff = current.Y - target.Y;
        var xDiff = current.X - target.X;

        var preferred = Direction.None;

        // Will be eg. yDiff = 2, xDiff = -4: [Right, Down, Up, Left]
        //
        // NOTE Abs(yDiff) == Abs(xDiff) favors yDiff
        // eg. yDiff = 3, xDiff = -3: [Down, Right, Left, Up]
        var preferredSeq = new Direction[4];

        // preferred direction is horizontal
        if (Math.Abs(yDiff) < Math.Abs(xDiff))
        {
            // decide worst and best
            if (xDiff < 0)
            {
                preferredSeq[0] = Direction.Right;
                preferredSeq[3] = Direction.Left;
            }
            else
            {
                preferredSeq[0] = Direction.Left;
                preferredSeq[3] = Direction.Right;
            }

            // decide 2nd best and 3rd best
            if (yDiff < 0)
            {
                preferredSeq[1] = Direction.Up;
                preferredSeq[2] = Direction.Down;
            }
            else
            {
                preferredSeq[1] = Direction.Down;
                preferredSeq[2] = Direction.Up;
            }
        }
        //preffered direction vertical
        else
        {
            // decide worst and best
            if (yDiff < 0)
            {
                preferredSeq[0] = Direction.Up;
                preferredSeq[3] = Direction.Down;
            }
            else
            {
                preferredSeq[0] = Direction.Down;
                preferredSeq[3] = Direction.Up;
            }

            // decide 2nd best and 3rd best
            if (xDiff < 0)
            {
                preferredSeq[1] = Direction.Right;
                preferredSeq[2] = Direction.Left;
            }
            else
            {
                preferredSeq[1] = Direction.Left;
                preferredSeq[2] = Direction.Right;
            }
        }

        foreach (var dir in preferredSeq)
        {
            if (!disallowed.Contains(dir))
            {
                preferred = dir;
                break;
            }
        }

        return preferred;
    }

    public static Position PositionAfterMove(Position current, Direction direction)
    {
        var newPos = new Position();

        switch (direction)
        {
            case Direction.Up:
                ++newPos.Y;
                break;
            case Direction.Down:
                --newPos.Y;
                break;
            case Direction.Right:
                ++newPos.X;
                break;
            case Direction.Left:
                --newPos.X;
                break;
            default:
                break;
        }

        return newPos;
    }
}