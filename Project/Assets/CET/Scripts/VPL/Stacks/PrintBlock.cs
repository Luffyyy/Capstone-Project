using System.Collections;
using TMPro;
using UnityEngine;

// A basic block to test the VPL system, it prints Hello World
// Possibly we'll allow it to be used ingame later
public class PrintBlock : StackBlock
{
    public ExpressionTray Tray;


    public override void Awake()
    {
        base.Awake();
    }

    public override void Activated(VPLZone zone)
    {
        base.Activated(zone);
        Tray.Activated(zone);
    }

    // Update is called once per frame
    public override IEnumerator Execute()
    {
        print(Tray.Evaluate());

        yield return null;
    }
}
