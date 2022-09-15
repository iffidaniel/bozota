using Bozota.Models.Map.Objects.Abstractions;

namespace Bozota.Models.Abstractions
{
    public interface IPlayer : IMapObject
    {
        public string Name { get; }

        public int Speed { get; set; }

        public int Ammo { get; set; }

        public int Materials { get; set; }

        public List<Tuple<PlayerAction, Direction>> Actions { get; }

        public bool HasEnoughAmmo(int amount);

        public void ReduceAmmo(int amount);

        public bool HasEnoughMaterials(int amount);

        public void ReduceMaterials(int amount);
    }
}
