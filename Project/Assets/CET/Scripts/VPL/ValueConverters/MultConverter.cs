using System;
using UnityEngine;

[CreateAssetMenu(fileName="MultConverter", menuName="VPL/Converters/Mult Converter")]
public class MultConverter : ValueConverter
{
    public override object Convert(params object[] input)
    {
        if (input[0] is double ldouble && input[1] is double rdouble)
        {
            return ldouble * rdouble;
        }

        throw new Exception("Multiplication is only defined for numbers!");
    }
}