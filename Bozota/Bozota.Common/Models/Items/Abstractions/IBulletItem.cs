namespace Bozota.Common.Models.Items.Abstractions;

public interface IBulletItem : IDamageItem
{
    public Direction Direction { get; set; }

    public int Speed { get; set; }
}
