using Bozota.Models.Abstractions;
using Bozota.Models.Map;
using Bozota.Models.Map.Items;
using Bozota.Models.Map.Objects;

namespace Bozota.Models
{
    public class GameState
    {
        public int MapXCellCount { get; private set; }

        public int MapYCellCount { get; private set; }

        public int TotalCellCount { get; private set; }

        public List<List<RenderId>> Map { get; private set; }

        public List<Player> Players { get; private set; }

        public List<WallObject> Walls { get; private set; }

        public List<BombObject> Bombs { get; private set; }

        public List<FireItem> FireItems { get; private set; }

        public List<HealthItem> HealthItems { get; private set; }

        public List<AmmoItem> AmmoItems { get; private set; }

        public List<MaterialsItem> MaterialsItems { get; private set; }

        public List<BulletItem> Bullets { get; private set; }


        public GameState(int xCellCount, int yCellCount)
        {
            MapXCellCount = xCellCount;
            MapYCellCount = yCellCount;
            TotalCellCount = MapXCellCount * MapYCellCount;

            Map = new();
            Players = new();
            Walls = new();
            Bombs = new();
            FireItems = new();
            HealthItems = new();
            AmmoItems = new();
            MaterialsItems = new();
            Bullets = new();
        }
    }
}
