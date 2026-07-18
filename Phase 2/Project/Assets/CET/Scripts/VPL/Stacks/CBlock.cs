using System.Collections;
using UnityEngine;

public class CBlock : BaseBlock
{
    public BlockTray Tray;

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
