using System;
using UnityEngine;

[CreateAssetMenu(fileName="GreaterThanConverter", menuName="VPL/Converters/Greater Than Converter")]
public class GreaterThanConverter : ValueConverter
{
    public override object Convert(params object[] input)
    {
        if (input[0] is double ldobule && input[1] is double rdouble)
        {
            return ldobule > rdouble;
        }

        throw new Exception("Greater Than is only defined for numbers!");
    }
}