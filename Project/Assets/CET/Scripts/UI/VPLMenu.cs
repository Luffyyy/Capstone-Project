using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VPLMenu : MenuBase
{
    public Dictionary<int, VPLZone> Zones = new();

    private VPLZone LastOpenZone;

    public void AddZone(VPLZone zone, int id)
    {
        Zones[id] = zone;
        zone.Hide();
    }

    public void OpenVPLZone(int id)
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
