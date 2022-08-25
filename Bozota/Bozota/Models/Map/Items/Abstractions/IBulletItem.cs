namespace Bozota.Models.Map.Items.Abstractions
{
    public interface IBulletItem : IMapItem
    {
        public Direction Direction { get; set; }

        public int Speed { get; set; }

        public int DamageAmount { get; set; }
    }
}
