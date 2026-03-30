using UnityEngine;

[CreateAssetMenu(fileName = "PrefabBlockDefinition", menuName = "VPL/Blocks/Prefab")]
public class PrefabBlockDefinition : BlockDefinition
{
    public string Prefab;

    void OnEnable()
    {
        PrefabName = Prefab;
    }
}
