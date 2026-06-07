using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Enter", menuName = "VPL/Functions/Enter")]
public class EnterFunction : FuncBlockDefinition
{
    public override List<VPLArg> Args => new()
    {
        new() { Name = "char", Type = "str" },
    };

    public override IEnumerator ExecuteAsync(params object[] input)
    {
        var co = Zone.ConnectedTo.ConnectedObjects;
        string inputStr = input[0].ToString();

        if (inputStr.Length == 1)
        {
            char c = input[0].ToString()[0];

            foreach (var obj in co)
            {
                var display = obj.GetComponent<PasswordDisplay>();
                display.Enter(c);
            }
        }

        yield return new WaitForSeconds(0.25f);
    }
}
