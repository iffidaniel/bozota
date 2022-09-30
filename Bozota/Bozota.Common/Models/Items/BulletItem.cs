using Bozota.Common.Models.Items.Abstractions;

namespace Bozota.Common.Models.Items;

public class BulletItem : IBulletItem
{
    public RenderId Render => RenderId.Bullet;

    public int XPos { get; set; }

    public int YPos { get; set; }

    public Direction Direction { get; set; }

    public int Speed { get; set; }

    public int DamageAmount { get; set; }
    public string PlayerName { get; set; } = string.Empty;

    public BulletItem() { }

    public BulletItem(int xpos, int ypos, Direction direction, int speed, int damageAmount, string playerName)
    {
        XPos = xpos;
        YPos = ypos;
        Direction = direction;
        Speed = speed;
        DamageAmount = damageAmount;
        PlayerName = playerName;
    }
}
