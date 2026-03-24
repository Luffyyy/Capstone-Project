using UnityEngine;

[CreateAssetMenu(fileName = "EventBlockDefinition", menuName = "VPL/Blocks/Event")]
public class EventBlockDefinition : BlockDefinition
{
    // The name of the event to subscribe to
    public string EventName;

    public override string PrefabName => "EventBlock";
}
