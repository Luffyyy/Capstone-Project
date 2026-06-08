using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class LevelFinished : MonoBehaviour
{
    public RectTransform panel;
    public AudioSource audioSource;

    public TextMeshProUGUI EscapedPlayers;

    private Animator anim;

    private void Awake()
    {
        gameObject.SetActive(false);
        anim = GetComponent<Animator>();
    }
    public void Show()
    {
        gameObject.SetActive(true);
        
        audioSource.Play();
        anim.Play("FinishedLevel");

        MenuManager.Instance.CloseCurrentMenu(); // So players see it
    }

    public void UpdateEscapedPlayers(int num)
    {
        EscapedPlayers.text = num + "/2 Players Escaped";
    }
}