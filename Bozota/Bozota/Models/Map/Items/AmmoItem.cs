using Bozota.Models.Common;
using Bozota.Models.Map.Items.Abstractions;

namespace Bozota.Models.Map.Items
{
    public class AmmoItem : IAmmoItem
    {
        public RenderId Render { get => RenderId.Ammo; }

        public int XPos { get; set; }

        public int YPos { get; set; }

        public int Amount { get; set; }

        public AmmoItem(int xpos, int ypos, int amount)
        {
            XPos = xpos;
            YPos = ypos;
            Amount = amount;
        }
    }
}
