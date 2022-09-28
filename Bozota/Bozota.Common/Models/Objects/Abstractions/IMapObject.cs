using Bozota.Common.Models.Items.Abstractions;

namespace Bozota.Common.Models.Objects.Abstractions;

public interface IMapObject : IMapItem
{
    public Health Health { get; }
}
