using System;
using UnityEngine;

[CreateAssetMenu(fileName="DivConverter", menuName="VPL/Converters/Div Converter")]
public class DivConverter : ValueConverter
{
    public override object Convert(params object[] input)
    {
        if (input[0] is double ldouble && input[1] is double rdouble)
        {
            return ldouble / rdouble;
        }

        throw new Exception("Division is only defined for numbers!");
    }
}