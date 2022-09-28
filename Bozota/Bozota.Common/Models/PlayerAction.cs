namespace Bozota.Common.Models;

public class PlayerAction
{
    public GameAction Action { get; set; }

    public Direction Direction { get; set; }

    public PlayerAction(GameAction action = GameAction.None, Direction direction = Direction.None)
    {
        Action = action;
        Direction = direction;
    }
}
