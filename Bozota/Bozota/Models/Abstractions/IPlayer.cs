using Bozota.Models.Map.Objects.Abstractions;

namespace Bozota.Models.Abstractions
{
    public interface IPlayer : IMapObject
    {
        public string Name { get; }

        public int MovementSpeed { get; set; }

        public int Ammo { get; set; }

        public List<Tuple<PlayerAction, Direction>> Actions { get; }
    }
}
