using System.Collections.Generic;
using UnityEngine;

public class BlockDefinition : ScriptableObject
{
    public string Name;
    public Color Color;

    [HideInInspector]
    public string PrefabName = "";
}
