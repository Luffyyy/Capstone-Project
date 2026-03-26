using System;
using UnityEngine;

[CreateAssetMenu(fileName="AddConverter", menuName="VPL/Converters/Add Converter")]
public class AddConverter : ValueConverter
{
    public override object Convert(params object[] input)
    {
        object lvalue = input[0];
        object rvalue = input[1];

        if (lvalue is float lfloat)
        {
            if (rvalue is float rfloat)
            {
                return lfloat + rfloat;
            } else if (rvalue is string rstr)
            {
                return lfloat + rstr; // Note: this returns a string
            }
        } else if (lvalue is string lstr)
        {
            return lstr + rvalue;
        }

        throw new Exception("Addition is only defined for numbers and strings!");
    }
}