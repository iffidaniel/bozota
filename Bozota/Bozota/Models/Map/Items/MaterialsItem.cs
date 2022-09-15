using Bozota.Models.Map.Items.Abstractions;

namespace Bozota.Models.Map.Items
{
    public class MaterialsItem : IMaterialsItem
    {
        public RenderId Render { get => RenderId.Materials; }

        public int XPos { get; set; }

        public int YPos { get; set; }

        public int Amount { get; set; }

        public MaterialsItem(int xpos, int ypos, int amount)
        {
            XPos = xpos;
            YPos = ypos;
            Amount = amount;
        }
    }
}
