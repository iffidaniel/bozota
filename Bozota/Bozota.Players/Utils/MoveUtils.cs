using System.Net;
using Bozota.Common.Models;

namespace Bozota.Players.Utils;

public static class MoveUtils
{
    /// <summary>
    /// Calculates the most optimal next move according to the current map
    /// </summary>
    /// <param name="current">where robot is stationed now</param>
    /// <param name="target">where robot needs to ge to</param>
    /// <param name="impassable">impassable positions like walls</param>
    /// <returns>best direction to go on next turn</returns>
    public static Direction MoveTowards(Position current, Position target, List<Position> impassable)
    {
        var positions = new List<Position>();
        var deadEnds = new List<Position>();

        var preferredNextDirection = new Direction();

        // fill positions list as long as not in the target position
        while (current.X == target.X && current.Y == target.Y)
        {
            var disallowed = new List<Direction>();

            var rerun = false;
            var newPos = new Position();

            // run as long as valid direction is found
            // invalid directions are towards:
            // - impassable
            // - dead end
            do
            {
                preferredNextDirection = bestDirection(current, target, disallowed);

                newPos = PositionAfterMove(current, preferredNextDirection);
                if (impassable.Contains(newPos) && deadEnds.Contains(newPos))
                {
                    disallowed.Add(preferredNextDirection);
                    rerun = true;
                }
                else
                {
                    rerun = false;
                }
            } while (rerun == true);

            //Unfinished. Use: [U,L,R,D]

            // check if position is already visited = running from dead end
            var oldPosition = positions.FirstOrDefault(x => x.X == newPos.X && x.Y == newPos.Y);
            if (oldPosition != null)
            {
                positions.Remove(oldPosition);
                deadEnds.Add(oldPosition); // position is an dead end
            }

            positions.Add(newPos);

            current = newPos;
        }

        // reach target position
        // filled position list with way there

        // return first move
        return preferredNextDirection;
    }

    private static Direction bestDirection(Position current, Position target, List<Direction> disallowed)
    {
        var yDiff = current.Y - target.Y;
        var xDiff = current.X - target.X;

        var preferred = Direction.None;

        if (Math.Abs(yDiff) < Math.Abs(xDiff))
        {
            //target is more to horizontal direction
            if (xDiff < 0)
            {
                if (!disallowed.Contains(Direction.Right))
                {
                    preferred = Direction.Right;
                }
                else
                {
                    if (yDiff < 0)
                    {
                        if (!disallowed.Contains(Direction.Up))
                        {
                            preferred = Direction.Up;
                        }
                        else
                        {
                            if (disallowed.Contains(Direction.Down))
                            {
                                preferred = Direction.Down;
                            }
                            else
                            {
                                preferred = Direction.Left;
                            }
                        }
                    }
                    else
                    {
                        if (!disallowed.Contains(Direction.Down))
                        {
                            preferred = Direction.Down;
                        }
                        else
                        {
                            preferred = Direction.Up;
                        }
                    }
                }
            }
            else if (!disallowed.Contains(Direction.Left))
            {
                preferred = Direction.Left;
            }
            else
            {
                //target is more to vertical direction or diagonal direction
                if (yDiff < 0)
                {
                    preferred = Direction.Up;
                }
                else
                {
                    preferred = Direction.Down;
                }
            }
        }
        else
        {
            //target is more to vertical direction or diagonal direction
            if (yDiff < 0 && !disallowed.Contains(Direction.Up))
            {
                preferred = Direction.Up;
            }
            else
            {
                preferred = Direction.Down;
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