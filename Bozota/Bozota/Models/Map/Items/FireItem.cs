using Bozota.Models.Map.Items.Abstractions;

namespace Bozota.Models.Map.Items
{
    public class FireItem : IFireItem
    {
        public RenderId Render { get => RenderId.Fire; }

        public int XPos { get; set; }

        public int YPos { get; set; }

        public int Duration { get; set; }

        public int DamageAmount { get; set; }

        public FireItem(int xpos, int ypos, int duration, int damageAmount)
        {
            XPos = xpos;
            YPos = ypos;
            Duration = duration;
            DamageAmount = damageAmount;
        }
    }
}
