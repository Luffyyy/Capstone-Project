using UnityEngine;

[CreateAssetMenu(menuName="Converters/Not Converter")]
public class NotConverter : ValueConverter
{
    public override object Convert(params object[] input) => !Helpers.VPLIsTrue(input[0]);
}

[CreateAssetMenu(menuName="Converters/And Converter")]
public class AndConverter : ValueConverter
{
    public override object Convert(params object[] input) => Helpers.VPLIsTrue(input[0]) && Helpers.VPLIsTrue(input[1]);
}