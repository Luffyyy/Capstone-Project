using UnityEngine;
using UnityEngine.Events;

public class Level : MonoBehaviour
{
    [Multiline]
    public string TutorialText;

    public UnityEvent OnTutorialFinishEvent;

    public void Start()
    {
        if (GameState.Instance != null && GameState.Instance.IsDedicatedServer && !GameState.Instance.isServer)
        {
            return;
        }

        TutorialDialog dialog = (TutorialDialog)MenuManager.Instance.GetDialog("TutorialDialog");

        dialog.TutorialText.text = TutorialText;
        dialog.OnTutorialFinishEvent = OnTutorialFinishEvent;

        dialog.Show();
    }
}
