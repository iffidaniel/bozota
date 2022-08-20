using Bozota.Models.Map.Items.Abstractions;

namespace Bozota.Models.Map.Items
{
    public class AmmoItem : IMapItem
    {
        public RenderId Render { get => RenderId.Ammo; }

        public int XPos { get; set; }

        public int YPos { get; set; }

        public int Capacity { get; }

        public AmmoItem(int xpos, int ypos, int capacity)
        {
            XPos = xpos;
            YPos = ypos;
            Capacity = capacity;
        }
    }
}
