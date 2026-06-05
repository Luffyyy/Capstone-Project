using System;
using UnityEngine;

[CreateAssetMenu(fileName="SubConverter", menuName="VPL/Converters/Sub Converter")]
public class SubConverter : ValueConverter
{
    public override object Convert(params object[] input)
    {
        if (input[0] is double ldouble && input[1] is double rdouble)
        {
            return ldouble + rdouble;
        }

        throw new Exception("Subtraction is only defined for numbers!");
    }
}