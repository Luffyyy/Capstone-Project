using System;
using UnityEngine;

[CreateAssetMenu(fileName="ModConverter", menuName="VPL/Converters/Mod Converter")]
public class ModConverter : ValueConverter
{
    public override object Convert(params object[] input)
    {
        if (input[0] is double ldouble && input[1] is double rdouble)
        {
            return ldouble % rdouble;
        }

        throw new Exception("Modulo is only defined for numbers!");
    }
}