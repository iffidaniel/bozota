namespace Bozota.Models.Map.Items.Abstractions
{
    public interface IBulletItem : IDamageItem, IMapItem
    {
        public Direction Direction { get; set; }

        public int Speed { get; set; }
    }
}
