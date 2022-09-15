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

    public int Speed { get; set; }

    public int Ammo { get; set; }

    public int Materials { get; set; }

    public List<Tuple<PlayerAction, Direction>> Actions { get; set; }

    public Player(string name, int xpos, int ypos, int healthAmount, int minHealthAmount, int maxHealthAmount, int speed, int startingAmmo, int startingMaterials)
    {
        Name = name;
        XPos = xpos;
        YPos = ypos;
        Health = new(healthAmount, maxHealthAmount, minHealthAmount);
        Speed = speed;
        Ammo = startingAmmo;
        Materials = startingMaterials;
        Actions = new List<Tuple<PlayerAction, Direction>> { new Tuple<PlayerAction, Direction>(PlayerAction.None, Direction.None) };
    }

    public bool HasEnoughAmmo(int amount)
    {
        return Ammo >= amount;
    }

    public void ReduceAmmo(int amount)
    {
        Ammo -= amount;

        if (Ammo < 0)
        {
            Ammo = 0;
        }
    }

    public bool HasEnoughMaterials(int amount)
    {
        return Materials >= amount;
    }

    public void ReduceMaterials(int amount)
    {
        Materials -= amount;

        if (Materials < 0)
        {
            Materials = 0;
        }
    }
}
