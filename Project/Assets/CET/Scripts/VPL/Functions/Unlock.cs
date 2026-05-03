using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Unlock", menuName = "VPL/Functions/Unlock")]
public class Unlock : FuncBlockDefinition
{
    public override List<VPLArg> Args => new()
    {
        new() { Name = "port", Type = "num"},
        new() { Name = "pass", Type = "str"}
    };
    public override void Execute(params object[] input)
    {
        var co = Zone.ConnectedTo.ConnectedObjects;
        var port = int.Parse(input[0].ToString());

        foreach (var obj in co)
        {
            var entity = obj.GetComponent<Entity>();
            if (entity.Port == port)
            {
                var plock = obj.GetComponent<PasswordLock>();
                plock.EnterPassword(input[1].ToString());
            }
        }
    }
}
