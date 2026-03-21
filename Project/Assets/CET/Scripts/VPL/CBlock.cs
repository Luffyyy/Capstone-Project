using UnityEngine;

public class CBlock : BaseBlock
{
    public BlockTray Tray;

    public override void SetName(string name)
    {
        
    }

    public override void SetColor(Color color)
    {
        
    }

    public override void Activated(VPLZone zone)
    {
        base.Activated(zone);
        Tray.enabled = true;
    }

    public override void Execute()
    {
        Tray.Execute();
    }
}
