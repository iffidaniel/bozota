using Bozota.Common.Models.Players.Abstractions;

namespace Bozota.Common.Models.Players;

public class Player : IPlayer
{
    public string Name { get; }

    public RenderId Render { get; set; }

    public int XPos { get; set; }

    public int YPos { get; set; }

    public Health Health { get; set; }

    public int Speed { get; set; }

    public int Ammo { get; set; }

    public int Materials { get; set; }

    public List<PlayerAction> Actions { get; set; }

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
        switch (name)
        {
            case "Daniel":
                Render = RenderId.Daniel; break;
            case "Veikko":
                Render = RenderId.Veikko; break;
            case "Krishna":
                Render = RenderId.Krishna; break;
            case "Raif":
                Render = RenderId.Raif; break;
            case "Ramesh":
                Render = RenderId.Ramesh; break;
            case "Riku":
                Render = RenderId.Riku; break;
            default:
                Render = RenderId.Player;
                break;
        }
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
