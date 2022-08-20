using Bozota.Models.Map.Items.Abstractions;

namespace Bozota.Models.Map.Objects.Abstractions
{
    public interface IMapObject : IMapItem
    {
        public Health Health { get; }
    }
}
