using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IsPrime", menuName = "VPL/Functions/IsPrime")]
public class IsPrime : FuncExpressionBlockDefinition
{
    public override List<VPLArg> Args => new()
    {
        new() { Name = "", Type = "num"}
    };
    public override object ExecuteWithReturn(params object[] input)
    {
        var n = int.Parse(input[0].ToString());
        if (n <= 1)
        {
            return false;
        }
        if (n == 2)
        {
            return true;
        }

        if (n % 2 == 0)
        {
            return false;
        }
        
        for (int i = 3; i * i <= n; i += 2)
        {
            if (n % i == 0)
            {
                return false;
            }
        }   
        return true;         
    }
}
