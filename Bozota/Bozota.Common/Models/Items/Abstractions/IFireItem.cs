namespace Bozota.Common.Models.Items.Abstractions;

public interface IFireItem : IDamageItem
{
    public int Duration { get; set; }
}
