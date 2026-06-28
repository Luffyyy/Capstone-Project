using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class TutorialDialog : Dialog
{
    public TextMeshProUGUI TutorialText;

    public UnityEvent OnTutorialFinishEvent;

    public override void Hide()
    {
        base.Hide();

        OnTutorialFinishEvent.Invoke();
    }
}