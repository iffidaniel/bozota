using Bozota.Models.Map.Objects.Abstractions;

namespace Bozota.Models.Abstractions
{
    public interface IPlayer : IMapObject
    {
        public string Name { get; }

        public List<PlayerMove> Moves { get; }
    }
}
