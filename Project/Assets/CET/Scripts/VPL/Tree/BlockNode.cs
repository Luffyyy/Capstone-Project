using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Rendering;

[Serializable]
public enum BlockType
{
    ROOT,
    STACK,
    EXPRESSION
}

[Serializable]
public struct KeyValue {
    public KeyValue(string key, string value)
    {
        Key = key;
        Value = value;
    }

    public string Key;

    public string Value;
}

[Serializable]
public class BlockNode
{
    public BlockNode()
    {
        Ident = Guid.NewGuid().ToString();
    }

    // Identity used to track changes on the block
    public string Ident;
    // The definition used to make this block
    public string DefinitionName;
    // Information stored on the block such as literal values
    public List<KeyValue> Data = new();

    public BlockType Type;

    public List<BlockTrayNode> Trays = new();

    public List<ExpressionTrayNode> ExpressionTrays = new();
}