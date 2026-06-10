using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SendMessage", menuName = "VPL/Functions/SendMessage")]
public class SendMessageFunction : FuncBlockDefinition
{
    public override List<VPLArg> Args => new()
    {
        new() { Name = "port", Type = "num" },
        new() { Name = "msg", Type = "str" },
    };

    public override IEnumerator ExecuteAsync(params object[] input)
    {
        var co = Zone.ConnectedTo.ConnectedObjects;

        var port = int.Parse(input[0].ToString());
        var msg = input[1].ToString();

        foreach (var obj in co)
        {
            Debug.Log(obj);
            if (obj.TryGetComponent<TerminalInteractable>(out var terminal))
            {
                Debug.Log(terminal.Port);
                Debug.Log(port);
                if (terminal.Port == port)
                {
                    terminal.OnVPLMessageReceived(msg);
                    yield return null;
                }
            }
            break;
        }

        yield return null;
    }
}
