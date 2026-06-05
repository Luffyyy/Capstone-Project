using UnityEngine;

[CreateAssetMenu(fileName="NotConverter", menuName="VPL/Converters/Not Converter")]
public class NotConverter : ValueConverter
{
    public override string RightType => "bool";
    public override object Convert(params object[] input) => !Helpers.VPLIsTrue(input[0]);
}