using System;
using UnityEngine;

[CreateAssetMenu(fileName="AddConverter", menuName="VPL/Converters/Add Converter")]
public class AddConverter : ValueConverter
{
    public override object Convert(params object[] input)
    {
        object lvalue = input[0];
        object rvalue = input[1];

        if (lvalue is double ldouble)
        {
            if (rvalue is double rdouble)
            {
                return ldouble + rdouble;
            } else if (rvalue is string rstr)
            {
                return ldouble.ToString("G15") + rstr; // Note: this returns a string
            }
        } else if (lvalue is string lstr)
        {
            if (rvalue is double rdouble)
            {
                return lstr + rdouble.ToString("G15"); // Note: this returns a string
            } else
            {
                return lstr + rvalue;
            }
        }

        throw new Exception("Addition is only defined for numbers and strings!");
    }
}