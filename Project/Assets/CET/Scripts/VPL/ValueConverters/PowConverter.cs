using System;
using UnityEngine;

[CreateAssetMenu(fileName="PowConverter", menuName="VPL/Converters/Pow Converter")]
public class PowConverter : ValueConverter
{
    public override object Convert(params object[] input)
    {
        if (input[0] is double ldouble && input[1] is double rdouble)
        {
            return Math.Pow(ldouble, rdouble);
        }

        throw new Exception("Power is only defined for numbers!");
    }
}