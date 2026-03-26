using System.Collections;
using UnityEngine;

public class CBlock : StackBlock
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
        Tray.Activated(zone);
    }

    public override IEnumerator Execute()
    {
        Tray.Execute();

        yield return null;
    }
}
