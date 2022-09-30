using Bozota.Common.Models;
using Bozota.Common.Models.Items.Abstractions;
using Bozota.Common.Models.Objects.Abstractions;
using Bozota.Common.Models.Players;

namespace Bozota.Players.Utils;

public class GameStateUtils
{
    public GameState GameState { get; private set; }

    public List<Position> TakenPositions { get; private set; }

    public GameStateUtils()
    {
        TakenPositions = new();
        GameState = new();
    }

    public void ProcessGameState(GameState gameState)
    {
        GameState = gameState;

        TakenPositions.Clear();
        TakenPositions.AddRange(GetImpassablePositions());
    }

    public Player? GetPlayerStats(string name)
    {
        foreach (var player in GameState.Players)
        {
            if (player.Name == name)
            {
                return player;
            }
        }

        return null;
    }

    public IAmmoItem? FindClosestAmmoItem(Position origin)
    {
        IAmmoItem? itemWithLeastDistance = null;
        if (GameState.AmmoItems.Count <= 0)
        {
            return itemWithLeastDistance;
        }

        var leastDistance = DataUtils.GetDistance(origin, new Position { X = GameState.AmmoItems[0].XPos, Y = GameState.AmmoItems[0].YPos });
        foreach (var item in GameState.AmmoItems)
        {
            var distance = DataUtils.GetDistance(origin, new Position { X = item.XPos, Y = item.YPos });
            if (distance < leastDistance)
            {
                leastDistance = distance;
                itemWithLeastDistance = item;
            }
        }

        return itemWithLeastDistance;
    }

    public T? FindClosestItem<T>(Position origin) where T : IMapItem
    {
        var itemWithLeastDistance = default(T?);

        var props = typeof(GameState).GetProperties();
        foreach (var prop in props)
        {
            var type = typeof(List<T>);
            if (prop.GetType() == type)
            {
                continue;
            }

            var itemList = (List<T>?)prop.GetValue(GameState);
            if (itemList == null || itemList.Count <= 0)
            {
                return itemWithLeastDistance;
            }

            var leastDistance = DataUtils.GetDistance(origin, new Position { X = itemList[0].XPos, Y = itemList[0].YPos });
            foreach (var item in itemList)
            {
                if (item != null)
                {
                    var distance = DataUtils.GetDistance(origin, new Position { X = item.XPos, Y = item.YPos });
                    if (distance < leastDistance)
                    {
                        leastDistance = distance;
                        itemWithLeastDistance = item;
                    }
                }
            }

            break;
        }

        return itemWithLeastDistance;
    }

    public List<Position> GetImpassablePositions()
    {
        var takenPositions = new List<Position>();

        AddTakenPositions(takenPositions, GameState.Players);
        AddTakenPositions(takenPositions, GameState.Bombs);
        AddTakenPositions(takenPositions, GameState.Walls);

        for (int y = 0; y < GameState.MapYCellCount; y++)
        {
            takenPositions.Add(new Position { X = GameState.MapXCellCount, Y = y });
        }

        for (int x = 0; x < GameState.MapXCellCount; x++)
        {
            takenPositions.Add(new Position { X = x, Y = GameState.MapYCellCount });
        }

        return takenPositions;
    }

    private static void AddTakenPositions<T>(List<Position> takenPositions, List<T> objects) where T : IMapObject
    {
        foreach (var item in objects)
        {
            takenPositions.Add(new Position { X = item.XPos, Y = item.YPos });
        }
    }
}
