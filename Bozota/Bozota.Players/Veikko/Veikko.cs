
using Bozota.Common.Models;
using Bozota.Players;

public class Veikko : IPlayingPlayer
{
    public string Name => "Veikko";

    public PlayerAction NextAction(GameState gameState)
    {
        var action = new PlayerAction(Name, GameAction.Move, Direction.Left);

        return action;
    }
}