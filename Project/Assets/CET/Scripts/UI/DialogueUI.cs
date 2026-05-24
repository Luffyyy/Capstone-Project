using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    public TextMeshProUGUI TextUI;

    public static DialogueUI Instance;

    private WaitForSeconds waitForSeconds;

    private Queue<string> queue = new();

    private Coroutine Current;

    void Awake()
    {
        Instance = this;
        waitForSeconds = new WaitForSeconds(0.001f);
    }

    public IEnumerator SayAnimation()
    {
        while (queue.TryDequeue(out var line))
        {
            TextUI.maxVisibleCharacters = 0;
            TextUI.text = line;
            for (int i=0; i<line.Length; i++)
            {
                TextUI.maxVisibleCharacters++;
                yield return null;
            }
            yield return new WaitForSeconds(0.1f * line.Length);
        }
        Current = null;
        yield return null;
    }
 
    public void Say(string line)
    {
        queue.Enqueue(line);
        Current ??= StartCoroutine(SayAnimation());
    }

    void Update()
    {
        
    }
}