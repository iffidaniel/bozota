namespace Bozota.Common.Models.Objects.Abstractions;

public interface IBombObject : IMapObject
{
    public int ExplosionDamage { get; set; }

    public int ExplosionRadius { get; set; }
}
