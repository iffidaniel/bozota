namespace Bozota.Models.Map.Items.Abstractions
{
    public interface IAmmoItem : IMapItem
    {
        public int Amount { get; set; }
    }
}
