using System.Collections.Generic;
using UnityEngine;

public enum BlockCateogory
{
    Function,
    Logic,
    Variable,
    Operator,
    Event
}

public class BlockDefinition : ScriptableObject
{
    public string Name;

    public BlockCateogory Category;

    public Color Color;
    
    public bool DefaultBlock = true;


    [HideInInspector]
    public string PrefabName = "";

}
