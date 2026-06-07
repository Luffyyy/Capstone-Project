using System;
using UnityEngine;

[CreateAssetMenu(fileName="IntDivConverter", menuName="VPL/Converters/Int Div Converter")]
public class IntegerDivConverter : ValueConverter
{
    public override object Convert(params object[] input)
    {
        if (input[0] is float lfloat && input[1] is float rfloat)
        {
            return (float)Math.Floor(lfloat / rfloat);
        }

        throw new Exception("Division is only defined for numbers!");
    }
}