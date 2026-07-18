using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MenuBase
{
    public Slider MasterVol;
    public Slider MusicVol;
    public Slider UIVol;
    public Slider SFXVol;

    void Start()
    {
        MasterVol.value = PlayerPrefs.GetFloat("MasterVol", 1);
        MusicVol.value = PlayerPrefs.GetFloat("MusicVol", 0.25f);
        UIVol.value = PlayerPrefs.GetFloat("UIVol", 1);
        SFXVol.value = PlayerPrefs.GetFloat("SFXVol", 1);
    }

    public void SetMasterVolume(float volume)
    {
        SetVolume("Master", volume);
    }

    public void SetMusicVolume(float volume)
    {
        SetVolume("Music", volume);
    }

    public void SetUIVolume(float volume)
    {
        SetVolume("UI", volume);
    }

    public void SetSFXVolume(float volume)
    {
        SetVolume("SFX", volume);
    }

    public void SetVolume(string type, float volume)
    {
        VolumeController.Instance.SetVolume(type, volume);
        PlayerPrefs.SetFloat($"{type}Vol", volume);
        PlayerPrefs.Save();
    }
}
