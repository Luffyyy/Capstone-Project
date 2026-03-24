using UnityEngine;

[CreateAssetMenu(fileName="OrConverter", menuName="VPL/Converters/Or Converter")]
public class OrConverter : ValueConverter
{
    public override object Convert(params object[] input) => Helpers.VPLIsTrue(input[0]) || Helpers.VPLIsTrue(input[1]);
}