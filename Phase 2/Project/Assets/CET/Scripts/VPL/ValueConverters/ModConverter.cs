using System;
using UnityEngine;

[CreateAssetMenu(fileName="ModConverter", menuName="VPL/Converters/Mod Converter")]
public class ModConverter : ValueConverter
{
    public override object Convert(params object[] input)
    {
        if (input[0] is float lfloat && input[1] is float rfloat)
        {
            return lfloat % rfloat;
        }

        throw new Exception("Modulo is only defined for numbers!");
    }
}