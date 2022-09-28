﻿using Bozota.Common.Models.Objects.Abstractions;

namespace Bozota.Common.Models.Objects;

public class WallObject : IWallObject
{
    public RenderId Render => RenderId.Wall;

    public int XPos { get; set; }

    public int YPos { get; set; }

    public Health Health { get; set; }

    public WallObject(int xpos, int ypos, int healthAmount, bool isIndestructable = false)
    {
        XPos = xpos;
        YPos = ypos;
        Health = new(healthAmount, default, default, isIndestructable);
    }
}