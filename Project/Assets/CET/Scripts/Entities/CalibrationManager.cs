using System.Collections;
using UnityEngine;

public class CalibrationManager : MonoBehaviour
{
    public FrequencyModule[] Modules;
    public Door DoorLock;
    public float[] Frequencies = new float[] {};
    public static CalibrationManager Instance { get; private set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Shuffle();
        Instance = this;
        UpdateVisuals();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Shuffle()
    {
        for (int i = Frequencies.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (Frequencies[i], Frequencies[j]) = (Frequencies[j], Frequencies[i]);
            SwapAudio(i, j);
        }
    }

    void SwapAudio(int a, int b)
    {
        AudioClip tmp = Modules[a].GetComponent<AudioSource>().clip;
        Modules[a].GetComponent<AudioSource>().clip = Modules[b].GetComponent<AudioSource>().clip;
        Modules[b].GetComponent<AudioSource>().clip =tmp;
    }
    public void UpdateVisuals()
    {
        for (int i = 0; i < Modules.Length; i++)
        {
            Modules[i].SetFrequency(Frequencies[i]);
        }
    }
    public void IsSorted(int left, int right)
    {
        for (int i = 0; i < Frequencies.Length - 1; i++)
        {
            if (Frequencies[i] > Frequencies[i + 1])
            {
                StartCoroutine(PlaySwappedNote(left, right));
                return;
            }
        }
        DoorLock.IsOn = true;
        StartCoroutine(PlayNotes());
    }
    private IEnumerator PlayNotes(){
        int j = 0;
            while(j<3)
            {
                for (int i = 0; i < Modules.Length; i++)
                {
                    Modules[i].Interact();
                    yield return new WaitForSeconds(0.5f);
                }
                j++;
            }
    }
    private IEnumerator PlaySwappedNote(int left, int right)
    {
        Modules[left].Interact();
        yield return new WaitForSeconds(0.5f);
        Modules[right].Interact();
        yield return new WaitForSeconds(0.5f);
    }
    public void SwitchAudio(int i, AudioClip newAudio)
    {
        Modules[i].GetComponent<AudioSource>().clip = newAudio;
    }
}
