using UnityEngine;

[CreateAssetMenu(fileName = "EventBlockDefinition", menuName = "VPL/Blocks/Event")]
public class EventBlockDefinition : BlockDefinition
{
    void OnEnable()
    {
        PrefabName = "EventBlock";
    }
    // The name of the event to subscribe to
    public string EventName;
}
