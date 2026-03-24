using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StackBlock : BaseBlock
{
    protected StackBlock NextBlock;

    // Events have no top port, they self initiate, for example.
    public bool hasTopPort = true;
    public bool hasBottomPort = true;

    // Executes the block
    public virtual IEnumerator Execute()
    {
        yield return null;
    }
}
