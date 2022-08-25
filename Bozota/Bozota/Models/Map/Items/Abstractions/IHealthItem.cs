namespace Bozota.Models.Map.Items.Abstractions
{
    public interface IHealthItem : IMapItem
    {
        public int HealAmount { get; set; }
    }
}
