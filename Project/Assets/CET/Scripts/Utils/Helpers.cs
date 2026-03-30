using System.Collections.Generic;
using UnityEngine;

public class Helpers
{
    public static List<T> GetComponentsInChildren<T>(Transform transform, int depth=1, List<T> comps=null)
    {
        depth--;
        comps ??= new();

        for (int i=0; i<transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent<T>(out var comp))
            {
                comps.Add(comp);
            }

            if (depth != 0)
            {
                GetComponentsInChildren<T>(transform, depth, comps);
            }
        }

        return comps;
    }
    /*
        Handles any value in c# and converst it to a boolean value
    */
    public static bool VPLIsTrue(object value)
    {
        if (value == null) return false;

        return value switch
        {
            bool b => b,
            int i => i != 0,
            float f => f != 0.0f,
            string s => !string.IsNullOrEmpty(s),
            System.Collections.ICollection c => c.Count > 0,
            _ => true // Default for objects is true
        };
    }
}