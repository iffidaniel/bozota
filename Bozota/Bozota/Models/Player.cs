namespace Bozota.Models;

public class Player
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = "player";

    public int XPos { get; set; } = 0;

    public int YPos { get; set; } = 0;
}
