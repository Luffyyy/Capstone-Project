using UnityEngine;
using UnityEngine.UI;

public class ButtonUISound : MonoBehaviour
{
    void Start()
    {
        var button = GetComponent<Button>();
        var audioSource = GetComponent<AudioSource>();
        button.onClick.AddListener(() => audioSource.Play());
    }
}
