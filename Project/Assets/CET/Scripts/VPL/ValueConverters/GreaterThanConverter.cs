using System;
using UnityEngine;

[CreateAssetMenu(fileName="GreaterThanConverter", menuName="VPL/Converters/Greater Than Converter")]
public class GreaterThanConverter : ValueConverter
{
    public override object Convert(params object[] input)
    {
        if (input[0] is float lfloat && input[1] is float rfloat)
        {
            return lfloat > rfloat;
        }

        throw new Exception("Greater Than is only defined for numbers!");
    }
}