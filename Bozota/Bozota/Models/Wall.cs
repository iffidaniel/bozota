using Bozota.Models.Abstractions;

namespace Bozota.Models
{
    public class Wall : IMapItem
    {
        public RenderId Render { get => RenderId.Wall; }

        public int XPos { get; set; } = 0;

        public int YPos { get; set; } = 0;

        public Health Health { get; set; }

        public Wall(bool isIndestructable = false)
        {
            Health = new(100, 100, 100, isIndestructable);
        }
    }
}
