namespace Bozota.Models
{
    public class GameMap
    {
        public int XCellCount { get; private set; }

        public int YCellCount { get; private set; }

        public List<List<CellItem>> Map { get; private set; }

        public List<Player> Players { get; private set; }

        public GameMap(int xCellCount, int yCellCount)
        {
            XCellCount = xCellCount;
            YCellCount = yCellCount;
            Map = new();
            Players = new();
        }
    }
}
