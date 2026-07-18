using System;
using UnityEngine;

[CreateAssetMenu(fileName="NegConverter", menuName="VPL/Converters/Neg Converter")]
public class NegConverter : ValueConverter
{
    public override object Convert(params object[] input)
    {
        if (input[0] is float lfloat)
        {
            return -lfloat;
        }

        throw new Exception("Negation is only defined for numbers!");
    }
}