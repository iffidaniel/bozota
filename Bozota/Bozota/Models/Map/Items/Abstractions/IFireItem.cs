namespace Bozota.Models.Map.Items.Abstractions
{
    public interface IFireItem : IDamageItem, IMapItem
    {
        public int Duration { get; set; }
    }
}
