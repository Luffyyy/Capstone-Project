using System.Collections;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PasswordDisplay : Entity
{
    public string Password;

    [HideInInspector, SyncVar(hook=nameof(OnEnteredPasswordChanged))]
    public string EnteredPassword = "";

    public TextMeshProUGUI EnteredPasswordText;

    public UnityEvent OnPasswordCorrect;

    private Animator animator;

    void Awake()
    {
        animator = EnteredPasswordText.GetComponent<Animator>();
    }

    public void Reset()
    {
        StartCoroutine(nameof(ResetAfterAnimation));
    }

    public void OnEnteredPasswordChanged(string oldVal, string newVal)
    {
        UpdateText();
    }

    private IEnumerator ResetAfterAnimation()
    {
        animator.Play("ResetState");
        yield return new WaitForSeconds(1.5f);
        animator.Play("Idle");
        EnteredPassword = "";
        UpdateText();
    }

    public void Enter(char c)
    {
        if (EnteredPassword.Length >= 4) Reset();

        EnteredPassword += c;

        if (EnteredPassword.Length == 4)
        {
            if (EnteredPassword == Password)
            {
                OnPasswordCorrect.Invoke();
            } else
            {
                Reset();
            }
        }

        UpdateText();
    }

    public void UpdateText()
    {
        string s = "";
        for(int i=0; i<4; i++)
        {
            s += EnteredPassword.Length > i ? EnteredPassword[i] + " " : "_ ";
        }

        EnteredPasswordText.text = s;
    }
}
