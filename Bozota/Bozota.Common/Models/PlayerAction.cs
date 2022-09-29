namespace Bozota.Common.Models;

public class PlayerAction
{
    public string Name { get; set; }

    public GameAction Action { get; set; }

    public Direction Direction { get; set; }

    public PlayerAction() { }

    public PlayerAction(string name, GameAction action = GameAction.None, Direction direction = Direction.None)
    {
        Name = name;
        Action = action;
        Direction = direction;
    }
}
