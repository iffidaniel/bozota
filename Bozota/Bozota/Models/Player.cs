using Bozota.Models.Abstractions;
using Bozota.Models.Map;

namespace Bozota.Models;

public class Player : IPlayer
{
    public string Name { get; }

    public RenderId Render { get => RenderId.Player; }

    public int XPos { get; set; }

    public int YPos { get; set; }

    public Health Health { get; set; }

    public int MovementSpeed { get; set; }

    public int Ammo { get; set; }

    public List<Tuple<PlayerAction, Direction>> Actions { get; set; }

    public Player(string name, int xpos, int ypos, int healthAmount, int minHealthAmount, int maxHealthAmount, int movementSpeed, int startingAmmo)
    {
        Name = name;
        XPos = xpos;
        YPos = ypos;
        Health = new(healthAmount, maxHealthAmount, minHealthAmount);
        MovementSpeed = movementSpeed;
        Ammo = startingAmmo;
        Actions = new();
    }
}
