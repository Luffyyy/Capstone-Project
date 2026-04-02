using System.Collections.Generic;
using UnityEngine;

public class VPLMenu : MenuBase
{
    public Dictionary<uint, VPLZone> Zones = new();

    private VPLZone LastOpenZone;

    public void AddZone(VPLZone zone, uint id)
    {
        Zones[id] = zone;
        zone.transform.SetParent(transform, false);
        zone.Hide();
    }

    public void OpenVPLZone(uint id)
    {
        if (LastOpenZone != null)
        {
            LastOpenZone.Hide();
            LastOpenZone = null;
        }

        LastOpenZone = Zones[id];
        LastOpenZone.Show();
    }
}
