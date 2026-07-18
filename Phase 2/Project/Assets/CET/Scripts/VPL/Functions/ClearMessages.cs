using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ClearMessagesFunction", menuName = "VPL/Functions/ClearMessagesFunction")]
public class ClearMessagesFunction : FuncBlockDefinition
{
    public override IEnumerator ExecuteAsync(params object[] input)
    {
        var co = Zone.ConnectedTo.ConnectedObjects;

        foreach (var obj in co)
        {
            var terminal = obj.GetComponent<TerminalInteractable>();
            terminal.ClearMessagesRecevied();
        }

        yield return null;
    }
}
