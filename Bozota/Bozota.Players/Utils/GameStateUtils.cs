using Bozota.Common.Models;
using Bozota.Common.Models.Items.Abstractions;
using Bozota.Common.Models.Objects.Abstractions;
using Bozota.Common.Models.Players;
using Bozota.Common.Models.Players.Abstractions;

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

    public IMaterialsItem? FindClosestMaterialsItem(Position origin)
    {
        if (GameState.MaterialsItems.Count <= 0)
        {
            return null;
        }

        IMaterialsItem? itemWithLeastDistance = GameState.MaterialsItems[0];
        var leastDistance = DataUtils.GetDistance(origin, new Position { X = GameState.MaterialsItems[0].XPos, Y = GameState.MaterialsItems[0].YPos });
        foreach (var item in GameState.MaterialsItems)
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

    public IHealthItem? FindClosestHealthItem(Position origin)
    {
        if (GameState.HealthItems.Count <= 0)
        {
            return null;
        }

        IHealthItem? itemWithLeastDistance = GameState.HealthItems[0];
        var leastDistance = DataUtils.GetDistance(origin, new Position { X = GameState.HealthItems[0].XPos, Y = GameState.HealthItems[0].YPos });
        foreach (var item in GameState.HealthItems)
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

    public IPlayer? FindClosestPlayer(Position origin, string ownPlayerName)
    {
        if (GameState.Players.Count <= 0)
        {
            return null;
        }

        IPlayer? itemWithLeastDistance = GameState.Players[0];
        var leastDistance = DataUtils.GetDistance(origin, new Position { X = GameState.Players[0].XPos, Y = GameState.Players[0].YPos });
        if (GameState.Players[0].Name == ownPlayerName)
        {
            itemWithLeastDistance = GameState.Players[1];
            leastDistance = DataUtils.GetDistance(origin, new Position { X = GameState.Players[1].XPos, Y = GameState.Players[1].YPos });
        }

        foreach (var player in GameState.Players)
        {
            var distance = DataUtils.GetDistance(origin, new Position { X = player.XPos, Y = player.YPos });
            if (distance < leastDistance && player.Name != ownPlayerName)
            {
                leastDistance = distance;
                itemWithLeastDistance = player;
            }
        }

        return itemWithLeastDistance;
    }

    public IAmmoItem? FindClosestAmmoItem(Position origin)
    {
        if (GameState.AmmoItems.Count <= 0)
        {
            return null;
        }

        IAmmoItem? itemWithLeastDistance = GameState.AmmoItems[0];
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
