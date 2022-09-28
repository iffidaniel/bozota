namespace Bozota.Common.Models.Items.Abstractions;

public interface IAmmoItem : IMapItem
{
    public int Amount { get; set; }
}
