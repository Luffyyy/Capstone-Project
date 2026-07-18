using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VPLStore", menuName = "VPL/Store")]
public class VPLStore : ScriptableObject
{
    // The prefabs of the blocks, not the actual placable blocks
    public List<BaseBlock> BlockPrefabs;

    public List<BlockDefinition> Definitions;

    public Dictionary<BlockCateogory, List<BlockDefinition>> GetCategorizedDefinitions()
    {
        Dictionary<BlockCateogory, List<BlockDefinition>> dict = new();
        foreach (var def in Definitions)
        {
            if (!dict.ContainsKey(def.Category))
            {
                dict[def.Category] = new();
            }

            dict[def.Category].Add(def);
        }

        return dict;
    }

    public BlockDefinition GetDefinitionByName(string name)
    {
        return Definitions.Find(definition => definition.name == name);
    }

    public BaseBlock GetPrefabForDefinition(BlockDefinition defintion)
    {
        return BlockPrefabs.Find(prefab => prefab.name == defintion.PrefabName);
    }
}
