using Bozota.Common.Models.Items.Abstractions;

namespace Bozota.Common.Models.Items;

public class AmmoItem : IAmmoItem
{
    public RenderId Render => RenderId.Ammo;

    public int XPos { get; set; }

    public int YPos { get; set; }

    public int Amount { get; set; }

    public AmmoItem() { }

    public AmmoItem(int xpos, int ypos, int amount)
    {
        XPos = xpos;
        YPos = ypos;
        Amount = amount;
    }
}
