using System;
using UnityEngine;

[CreateAssetMenu(fileName="LessThanConverter", menuName="VPL/Converters/Less Than Converter")]
public class LessThanConverter : ValueConverter
{
    public override object Convert(params object[] input)
    {
        if (input[0] is double ldouble && input[1] is double rdobule)
        {
            return ldouble < rdobule;
        }

        throw new Exception("Less Than is only defined for numbers!");
    }
}