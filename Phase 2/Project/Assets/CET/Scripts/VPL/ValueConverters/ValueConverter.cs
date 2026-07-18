using UnityEngine;

public abstract class ValueConverter : ScriptableObject
{
    public virtual string LeftType => "num";
    public virtual string RightType => "num";
    public abstract object Convert(params object[] input);
}