using System;
using UnityEngine;

[CreateAssetMenu(fileName="MultConverter", menuName="VPL/Converters/Mult Converter")]
public class MultConverter : ValueConverter
{
    public override object Convert(params object[] input)
    {
        if (input[0] is float lfloat && input[1] is float rfloat)
        {
            return lfloat * rfloat;
        }

        throw new Exception("Multiplication is only defined for numbers!");
    }
}