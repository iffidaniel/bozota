using Bozota.Common.Models.Items;
using Bozota.Common.Models.Objects;
using Bozota.Common.Models.Players;

namespace Bozota.Common.Models;

public class GameState
{
    public int MapXCellCount { get; set; }

    public int MapYCellCount { get; set; }

    public int TotalCellCount { get; set; }

    public List<List<RenderId>> Map { get; set; }

    public List<Player> Players { get; set; }

    public List<WallObject> Walls { get; set; }

    public List<BombObject> Bombs { get; set; }

    public List<FireItem> FireItems { get; set; }

    public List<HealthItem> HealthItems { get; set; }

    public List<AmmoItem> AmmoItems { get; set; }

    public List<MaterialsItem> MaterialsItems { get; set; }

    public List<BulletItem> Bullets { get; set; }

    public GameState()
    {
    }

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
