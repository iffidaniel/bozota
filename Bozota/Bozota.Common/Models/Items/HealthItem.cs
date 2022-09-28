using Bozota.Common.Models.Items.Abstractions;

namespace Bozota.Common.Models.Items;

public class HealthItem : IHealthItem
{
    public RenderId Render => RenderId.Health;

    public int XPos { get; set; }

    public int YPos { get; set; }

    public int HealAmount { get; set; }

    public HealthItem(int xpos, int ypos, int healAmount)
    {
        XPos = xpos;
        YPos = ypos;
        HealAmount = healAmount;
    }
}
