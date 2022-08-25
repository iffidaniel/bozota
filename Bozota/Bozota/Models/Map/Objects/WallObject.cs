using Bozota.Models.Map.Objects.Abstractions;

namespace Bozota.Models.Map.Objects
{
    public class WallObject : IWallObject
    {
        public RenderId Render { get => RenderId.Wall; }

        public int XPos { get; set; }

        public int YPos { get; set; }

        public Health Health { get; set; }

        public WallObject(int xpos, int ypos, int healthAmount, bool isIndestructable = false)
        {
            XPos = xpos;
            YPos = ypos;
            Health = new(healthAmount, default, default, isIndestructable);
        }
    }
}
