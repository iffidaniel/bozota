using Bozota.Models.Abstractions;

namespace Bozota.Models
{
    public class GameMap
    {
        public int XCellCount { get; private set; }

        public int YCellCount { get; private set; }

        public List<List<RenderId>> Map { get; private set; }

        public List<IMapItem> Items { get; private set; }

        public List<Player> Players { get; private set; }

        public GameMap(int xCellCount, int yCellCount)
        {
            XCellCount = xCellCount;
            YCellCount = yCellCount;

            Map = new();
            Items = new();
            Players = new();
        }
    }
}
