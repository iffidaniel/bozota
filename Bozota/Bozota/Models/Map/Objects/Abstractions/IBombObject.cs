namespace Bozota.Models.Map.Objects.Abstractions
{
    public interface IBombObject : IMapObject
    {
        public int ExplosionDamage { get; set; }

        public int ExplosionRadius { get; set; }
    }
}
