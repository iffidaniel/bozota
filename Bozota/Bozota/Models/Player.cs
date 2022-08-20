using Bozota.Models.Abstractions;
using Bozota.Models.Map;

namespace Bozota.Models;

public class Player : IPlayer
{
    public string Name { get; }

    public RenderId Render { get => RenderId.Player; }

    public int XPos { get; set; }

    public int YPos { get; set; }

    public Health Health { get; }

    public List<PlayerMove> Moves { get; }

    public Player(string name, int xpos, int ypos)
    {
        Name = name;
        XPos = xpos;
        YPos = ypos;
        Health = new(100, 200, 10);
        Moves = new();
    }
}
