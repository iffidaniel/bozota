using Bozota.Models.Map.Items.Abstractions;

namespace Bozota.Models.Map.Items
{
    public class BulletItem : IMapItem
    {
        public RenderId Render { get => RenderId.Bullet; }

        public int XPos { get; set; } = 0;

        public int YPos { get; set; } = 0;

        public int DamageAmount { get; } = 0;

        public BulletItem(int xpos, int ypos, int damageAmount)
        {
            XPos = xpos;
            YPos = ypos;
            DamageAmount = damageAmount;
        }
    }
}
