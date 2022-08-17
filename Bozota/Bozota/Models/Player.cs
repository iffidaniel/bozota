using Bozota.Models.Abstractions;

namespace Bozota.Models;

public class Player : IMapItem
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; }

    public RenderId Render { get => RenderId.Player; }

    public int XPos { get; set; } = 0;

    public int YPos { get; set; } = 0;

    public Health Health { get; set; }

    public List<string> Moves;

    public Player(string name)
    {
        Name = name;
        Health = new(100, 200, 10);
        Moves = new();
    }
}
