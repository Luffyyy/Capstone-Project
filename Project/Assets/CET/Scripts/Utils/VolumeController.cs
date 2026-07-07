using UnityEngine;
using UnityEngine.Audio;

public class VolumeController : MonoBehaviour
{
    public AudioMixer MasterMixer;

    public static VolumeController Instance;

    void Start()
    {
        Instance = this;

        SetVolume("Music", 0);
        SetVolume("SFX", 0);
    }

    public void SetDefaults()
    {
        SetVolume("Music", PlayerPrefs.GetFloat("MusicVol", 0.25f));
        SetVolume("SFX", PlayerPrefs.GetFloat("SFXVol", 1));
        SetVolume("UI", PlayerPrefs.GetFloat("UIVol", 1));
    }

    private static float Decibels(float value)
    {
        return Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
    }

    public void SetVolume(string type, float value)
    {
        MasterMixer.SetFloat(type+"Vol", Decibels(value));
    }
}
