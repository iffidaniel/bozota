using Bozota.Common.Models.Players.Abstractions;

namespace Bozota.Common.Models.Players;

public class Player : IPlayer
{
    public string Name { get; set; } = string.Empty;

    public RenderId Render { get; set; } = RenderId.Player;

    public int XPos { get; set; }

    public int YPos { get; set; }

    public Health Health { get; set; } = new();

    public int Speed { get; set; }

    public int Ammo { get; set; }

    public int Materials { get; set; }

    public List<PlayerAction> Actions { get; set; } = new();

    public Player() { }

    public Player(string name, int xpos, int ypos, int healthAmount, int minHealthAmount, int maxHealthAmount, int speed, int startingAmmo, int startingMaterials)
    {
        Name = name;
        XPos = xpos;
        YPos = ypos;
        Health = new(healthAmount, maxHealthAmount, minHealthAmount);
        Speed = speed;
        Ammo = startingAmmo;
        Materials = startingMaterials;
        Actions = new List<PlayerAction>
        {
            new PlayerAction(name)
        };
        Render = name switch
        {
            "Daniel" => RenderId.Daniel,
            "Veikko" => RenderId.Veikko,
            "Krishna" => RenderId.Krishna,
            "Raif" => RenderId.Raif,
            "Ramesh" => RenderId.Ramesh,
            "Riku" => RenderId.Riku,
            _ => RenderId.Player,
        };
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
