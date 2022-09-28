namespace Bozota.Common.Models.Items.Abstractions;

public interface IDamageItem : IMapItem
{
    public int DamageAmount { get; set; }
}
