using System;
using UnityEngine;

[CreateAssetMenu(fileName="SubConverter", menuName="VPL/Converters/Sub Converter")]
public class SubConverter : ValueConverter
{
    public override object Convert(params object[] input)
    {
        if (input[0] is float lfloat && input[1] is float rfloat)
        {
            return lfloat + rfloat;
        }

        throw new Exception("Subtraction is only defined for numbers!");
    }
}