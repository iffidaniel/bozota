using Bozota.Common.Models.Items.Abstractions;

namespace Bozota.Common.Models.Items;

public class MaterialsItem : IMaterialsItem
{
    public RenderId Render => RenderId.Materials;

    public int XPos { get; set; }

    public int YPos { get; set; }

    public int Amount { get; set; }

    public MaterialsItem() { }

    public MaterialsItem(int xpos, int ypos, int amount)
    {
        XPos = xpos;
        YPos = ypos;
        Amount = amount;
    }
}
