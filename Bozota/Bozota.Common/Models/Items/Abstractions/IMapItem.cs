namespace Bozota.Common.Models.Items.Abstractions;

public interface IMapItem
{
    public RenderId Render { get; }

    public int XPos { get; set; }

    public int YPos { get; set; }
}
