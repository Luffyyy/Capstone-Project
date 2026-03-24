using System.Collections;
using TMPro;
using UnityEngine;

// A basic block to test the VPL system, it prints Hello World
// Possibly we'll allow it to be used ingame later
public class PrintBlock : StackBlock
{
    public TMP_InputField ValueField;

    public override void Awake()
    {
        base.Awake();
        ValueField.interactable = false;
    }

    public override void Activated(VPLZone zone)
    {
        base.Activated(zone);
        ValueField.interactable = true;
    }

    // Update is called once per frame
    public override IEnumerator Execute()
    {
        print(ValueField.text);

        yield return null;
    }
}
