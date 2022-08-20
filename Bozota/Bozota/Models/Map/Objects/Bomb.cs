using Bozota.Models.Map.Objects.Abstractions;

namespace Bozota.Models.Map.Objects
{
    public class Bomb : IMapObject
    {
        public RenderId Render { get => RenderId.Bomb; }

        public int XPos { get; set; }

        public int YPos { get; set; }

        public Health Health { get; }

        public int ExplosionDamage { get; }

        public int ExplosionRadius { get; }

        public Bomb(int xpos, int ypos, int damage, int radius)
        {
            XPos = xpos;
            YPos = ypos;
            Health = new(20);
            ExplosionDamage = damage;
            ExplosionRadius = radius;
        }
    }
}
