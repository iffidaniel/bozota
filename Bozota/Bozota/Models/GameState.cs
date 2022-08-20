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

        public List<List<RenderId>> Map { get; private set; }

        public List<IMapItem> Items { get; private set; }

        public List<IMapObject> Objects { get; private set; }

        public List<IPlayer> Players { get; private set; }

        public GameState(int xCellCount, int yCellCount)
        {
            MapXCellCount = xCellCount;
            MapYCellCount = yCellCount;

            Map = new();
            Items = new();
            Objects = new();
            Players = new();
        }
    }
}
