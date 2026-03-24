using UnityEngine;

[CreateAssetMenu(fileName="AndConverter", menuName="VPL/Converters/And Converter")]
public class AndConverter : ValueConverter
{
    public override object Convert(params object[] input) => Helpers.VPLIsTrue(input[0]) && Helpers.VPLIsTrue(input[1]);
}