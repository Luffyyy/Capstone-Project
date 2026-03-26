using UnityEngine;

public abstract class ValueConverter : ScriptableObject
{
    public abstract object Convert(params object[] input);
}