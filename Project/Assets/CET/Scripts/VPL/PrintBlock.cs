using TMPro;
using UnityEngine;

// A basic block to test the VPL system, it prints Hello World
// Possibly we'll allow it to be used ingame later
public class PrintBlock : BaseBlock
{
    public TMP_InputField ValueField;

    // Update is called once per frame
    public override void Execute()
    {
        print(ValueField.text);
    }
}
