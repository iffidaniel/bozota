using Bozota.Models.Common;

namespace Bozota.Models.Map.Players
{
    public class PlayerAction
    {
        public GameAction Action { get; set; } = GameAction.None;

        public Direction Direction { get; set; } = Direction.None;

        public PlayerAction(GameAction action = GameAction.None, Direction direction = Direction.None)
        {
            Action = action;
            Direction = direction;
        }
    }
}
