using UnityEngine;

[CreateAssetMenu(fileName = "OperatorDefinition", menuName = "VPL/Blocks/Operator")]
public class OperatorDefinition : BlockDefinition
{
    // The converter that handles the operator's functionality
    public ValueConverter Converter;

    // Whether the operator acts upon a single value (the 2nd expression) or an operation between the two expressions
    public bool IsUnary = false;

    // The sign, such as +, NOT, AND, etc
    public string Sign;

    public override string PrefabName => "OperatorBlock";
}