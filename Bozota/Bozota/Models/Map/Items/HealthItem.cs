using Bozota.Models.Common;
using Bozota.Models.Map.Items.Abstractions;

namespace Bozota.Models.Map.Items
{
    public class HealthItem : IHealthItem
    {
        public RenderId Render { get => RenderId.Health; }

        public int XPos { get; set; }

        public int YPos { get; set; }

        public int HealAmount { get; set; }

        public HealthItem(int xpos, int ypos, int healAmount)
        {
            XPos = xpos;
            YPos = ypos;
            HealAmount = healAmount;
        }
    }
}
