using System;
using UnityEngine;

[CreateAssetMenu(fileName="LessThanConverter", menuName="VPL/Converters/Less Than Converter")]
public class LessThanConverter : ValueConverter
{
    public override object Convert(params object[] input)
    {
        if (input[0] is float lfloat && input[1] is float rfloat)
        {
            return lfloat < rfloat;
        }

        throw new Exception("Less Than is only defined for numbers!");
    }
}