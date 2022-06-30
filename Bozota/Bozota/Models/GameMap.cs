namespace Bozota.Models
{
    public class GameMap
    {
        public int XCellCount { get; private set; }

        public int YCellCount { get; private set; }

        public List<List<CellItem>> Map { get; private set; }

        public GameMap(int rowCount, int columnCount)
        {
            XCellCount = rowCount;
            YCellCount = columnCount;
            Map = new();
        }
    }
}
