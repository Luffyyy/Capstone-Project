using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VPLStore", menuName = "VPL/Store")]
public class VPLStore : ScriptableObject
{
    // The prefabs of the blocks, not the actual placable blocks
    public List<BaseBlock> BlockPrefabs;

    public List<BlockDefinition> Definitions;

    public BaseBlock GetPrefabForDefinition(BlockDefinition defintion)
    {
        return BlockPrefabs.Find(prefab => prefab.name == defintion.PrefabName);
    }
}
