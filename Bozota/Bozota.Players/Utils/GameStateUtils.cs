using Bozota.Common.Models;
using Bozota.Common.Models.Items.Abstractions;
using Bozota.Common.Models.Objects.Abstractions;
using Bozota.Common.Models.Players;

namespace Bozota.Players.Utils;

public class GameStateUtils
{
    private GameState _gameState = new();

    public List<Position> TakenPositions { get; private set; }

    public GameStateUtils()
    {
        TakenPositions = new();
    }

    public void ProcessGameState(GameState gameState)
    {
        _gameState = gameState;

        TakenPositions.Clear();
        TakenPositions.AddRange(FindTakenPositions());
    }

    public Player? GetPlayerStats(string name)
    {
        foreach (var player in _gameState.Players)
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
        if (_gameState.AmmoItems.Count <= 0)
        {
            return itemWithLeastDistance;
        }

        var leastDistance = DataUtils.GetDistance(origin, new Position { X = _gameState.AmmoItems[0].XPos, Y = _gameState.AmmoItems[0].YPos });
        foreach (var item in _gameState.AmmoItems)
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

            var itemList = (List<T>?)prop.GetValue(_gameState);
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

    private List<Position> FindTakenPositions()
    {
        var takenPositions = new List<Position>();

        AddTakenPositions(takenPositions, _gameState.Players);
        AddTakenPositions(takenPositions, _gameState.Bombs);
        AddTakenPositions(takenPositions, _gameState.Walls);

        for (int y = 0; y < _gameState.MapYCellCount; y++)
        {
            takenPositions.Add(new Position { X = _gameState.MapXCellCount, Y = y });
        }

        for (int x = 0; x < _gameState.MapXCellCount; x++)
        {
            takenPositions.Add(new Position { X = x, Y = _gameState.MapYCellCount });
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
