using Bozota.Models.Map.Objects.Abstractions;

namespace Bozota.Models.Map.Objects
{
    public class Wall : IMapObject
    {
        public RenderId Render { get => RenderId.Wall; }

        public int XPos { get; set; }

        public int YPos { get; set; }

        public Health Health { get; }

        public Wall(int xpos, int ypos, bool isIndestructable = false)
        {
            XPos = xpos;
            YPos = ypos;
            Health = new(200, default, default, isIndestructable);
        }
    }
}
