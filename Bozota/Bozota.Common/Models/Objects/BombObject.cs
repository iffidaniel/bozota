using Bozota.Common.Models.Objects.Abstractions;

namespace Bozota.Common.Models.Objects;

public class BombObject : IBombObject
{
    public RenderId Render => RenderId.Bomb;

    public int XPos { get; set; }

    public int YPos { get; set; }

    public Health Health { get; set; }

    public int ExplosionDamage { get; set; }

    public int ExplosionRadius { get; set; }

    public BombObject() { }

    public BombObject(int xpos, int ypos, int healthAmount, int damage, int radius)
    {
        XPos = xpos;
        YPos = ypos;
        Health = new(healthAmount);
        ExplosionDamage = damage;
        ExplosionRadius = radius;
    }
}
