using Bozota.Models.Map.Items.Abstractions;

namespace Bozota.Models.Map.Items
{
    public class BulletItem : IBulletItem
    {
        public RenderId Render { get => RenderId.Bullet; }

        public int XPos { get; set; }

        public int YPos { get; set; }

        public Direction Direction { get; set; }

        public int Speed { get; set; }

        public int DamageAmount { get; set; }

        public BulletItem(int xpos, int ypos, Direction direction, int speed, int damageAmount)
        {
            XPos = xpos;
            YPos = ypos;
            Direction = direction;
            Speed = speed;
            DamageAmount = damageAmount;
        }
    }
}
