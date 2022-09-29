using Bozota.Common.Models;
using Bozota.Common.Models.Objects.Abstractions;

namespace Bozota.Players;

public class PlayerUtils
{
    public List<Position> TakenPositions { get; private set; }

    public PlayerUtils()
    {
        TakenPositions = new();
    }

    public Task ProcessGameState(GameState gameState)
    {
        TakenPositions.Clear();
        TakenPositions = FindTakenPositions(gameState);

        return Task.CompletedTask;
    }

    private static List<Position> FindTakenPositions(GameState gameState)
    {
        var takenPositions = new List<Position>();

        AddTakenPositions(takenPositions, gameState.Players);
        AddTakenPositions(takenPositions, gameState.Bombs);
        AddTakenPositions(takenPositions, gameState.Walls);

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
