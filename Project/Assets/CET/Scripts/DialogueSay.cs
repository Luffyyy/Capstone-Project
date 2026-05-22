using System.Collections.Generic;
using UnityEngine;

public class DialogueSay : MonoBehaviour
{
    [Multiline]
    public List<string> Lines;
    public bool SayOnAwake = true;
    
    public bool SayOnce = true;

    private bool said = false;

    void Start()
    {
        if (SayOnAwake)
        {
            Say();
        }
    }

    public void Say()
    {
        if (SayOnce && said) return;


        said = true;
        foreach (var line in Lines)
        {
            print(line);
            DialogueUI.Instance.Say(line);
        }
    }
}
