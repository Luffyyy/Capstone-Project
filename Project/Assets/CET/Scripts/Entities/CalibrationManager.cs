using UnityEngine;

public class CalibrationManager : MonoBehaviour
{
    public FrequencyModule[] Modules;
    public float[] Frequencies = new float[] {};
    public static CalibrationManager Instance { get; private set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        UpdateVisuals();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateVisuals()
    {
        for (int i = 0; i < Modules.Length; i++)
        {
            Modules[i].SetFrequency(Frequencies[i]);
        }
    }
    public void IsSorted()
    {
        for (int i = 0; i < Frequencies.Length - 1; i++)
        {
            if (Frequencies[i] > Frequencies[i + 1])
            {
                return;
            }
        }
        PlayNotes();
    }
    public void PlayNotes(){
        for (int i = 0; i < Modules.Length; i++)
        {
            Modules[i].Interact();
        }
    }
    public void SwitchAudio(int i, AudioClip newAudio)
    {
        Modules[i].GetComponent<AudioSource>().clip = newAudio;
    }
}
