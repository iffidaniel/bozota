using Bozota.Models.Map.Items.Abstractions;

namespace Bozota.Models.Map.Items
{
    public class HealthItem : IMapItem
    {
        public RenderId Render { get => RenderId.Health; }

        public int XPos { get; set; } = 0;

        public int YPos { get; set; } = 0;

        public int HealAmount { get; } = 0;

        public HealthItem(int xpos, int ypos, int healAmount)
        {
            XPos = xpos;
            YPos = ypos;
            HealAmount = healAmount;
        }
    }
}
