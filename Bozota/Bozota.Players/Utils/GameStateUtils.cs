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
        TakenPositions.AddRange(FindTakenPositions());
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
        var leastDistance = 0;

        foreach (var ammo in GameState.AmmoItems)
        {
            var distance = DataUtils.GetDistance(origin, new Position { X = ammo.XPos, Y = ammo.YPos });
            if (distance < leastDistance)
            {
                leastDistance = distance;
            }
        }

        return null;
    }

    private List<Position> FindTakenPositions()
    {
        var takenPositions = new List<Position>();

        AddTakenPositions(takenPositions, GameState.Players);
        AddTakenPositions(takenPositions, GameState.Bombs);
        AddTakenPositions(takenPositions, GameState.Walls);

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
