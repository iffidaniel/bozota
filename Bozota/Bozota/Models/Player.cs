namespace Bozota.Models;

public class Player
{
    public Guid Id { get; set; }

    public string Name { get; set; } = "player";

    public int xPos { get; set; } = 0;

    public int yPos { get; set; } = 0;
}
