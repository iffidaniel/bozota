using Bozota.Common.Models;
using Bozota.Players.Utils;

namespace Bozota.Players;

public interface IPlayingPlayer
{
    public string Name { get; }

    public PlayerAction NextAction(GameStateUtils gameStateUtils);
}
