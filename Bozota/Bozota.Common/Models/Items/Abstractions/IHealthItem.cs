namespace Bozota.Common.Models.Items.Abstractions;

public interface IHealthItem : IMapItem
{
    public int HealAmount { get; set; }
}
