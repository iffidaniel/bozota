namespace Bozota.Models.Map.Items.Abstractions
{
    public interface IDamageItem : IMapItem
    {
        public int DamageAmount { get; set; }
    }
}
