using Bozota.Common.Models;

namespace Bozota.Players;

public interface IPlayingPlayer
{
    public string Name { get; }

    public PlayerAction NextAction(GameState gameState, PlayerUtils playerUtils);
}
