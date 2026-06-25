using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class LineDefinition
{
    [Multiline]
    public string Line;

    public AudioClip Clip;
}

public class DialogueUI : MonoBehaviour
{
    public TextMeshProUGUI TextUI;

    public static DialogueUI Instance;

    private WaitForSeconds waitForSeconds;

    private Queue<LineDefinition> queue = new();

    private Coroutine Current;

    private AudioSource audioSource;

    void Awake()
    {
        Instance = this;
        waitForSeconds = new WaitForSeconds(0.001f);
        audioSource = GetComponent<AudioSource>();
    }

    public IEnumerator SayAnimation()
    {
        while (queue.TryDequeue(out var line))
        {
            TextUI.maxVisibleCharacters = 0;
            var lineStr = line.Line;
            TextUI.text = lineStr;

            audioSource.clip = line.Clip;
            audioSource.Play();
            
            for (int i=0; i<lineStr.Length; i++)
            {
                TextUI.maxVisibleCharacters++;
                yield return null;
            }

            yield return new WaitForSeconds(0.07f * lineStr.Length);
        }

        TextUI.text = "";

        Current = null;
        yield return null;
    }
 
    public void Say(LineDefinition line)
    {
        queue.Enqueue(line);
        Current ??= StartCoroutine(SayAnimation());
    }

    public void Interrupt()
    {
        if (Current != null)
        {
            StopCoroutine(Current);
            Current = null;
        }
        queue.Clear();
    }

    void Update()
    {
        
    }
}