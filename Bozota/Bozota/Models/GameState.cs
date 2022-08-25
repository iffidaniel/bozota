using Bozota.Models.Abstractions;
using Bozota.Models.Map;
using Bozota.Models.Map.Items.Abstractions;
using Bozota.Models.Map.Objects.Abstractions;

namespace Bozota.Models
{
    public class GameState
    {
        public int MapXCellCount { get; private set; }

        public int MapYCellCount { get; private set; }

        public int TotalCellCount { get; private set; }

        public List<List<RenderId>> Map { get; private set; }

        public List<IPlayer> Players { get; private set; }

        public List<IWallObject> Walls { get; private set; }

        public List<IBombObject> Bombs { get; private set; }

        public List<IHealthItem> HealthItems { get; private set; }

        public List<IAmmoItem> AmmoItems { get; private set; }

        public List<IBulletItem> Bullets { get; private set; }


        public GameState(int xCellCount, int yCellCount)
        {
            MapXCellCount = xCellCount;
            MapYCellCount = yCellCount;
            TotalCellCount = MapXCellCount * MapYCellCount;

            Map = new();
            Players = new();
            Walls = new();
            Bombs = new();
            HealthItems = new();
            AmmoItems = new();
            Bullets = new();
        }
    }
}
