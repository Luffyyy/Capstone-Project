using System;
using UnityEngine;

[CreateAssetMenu(fileName="PowConverter", menuName="VPL/Converters/Pow Converter")]
public class PowConverter : ValueConverter
{
    public override object Convert(params object[] input)
    {
        if (input[0] is float lfloat && input[1] is float rfloat)
        {
            return Math.Pow(lfloat, rfloat);
        }

        throw new Exception("Power is only defined for numbers!");
    }
}