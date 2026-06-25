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

    public string PlaceHolder = "_";

    public bool Reverse;

    void Awake()
    {
        animator = EnteredPasswordText.GetComponent<Animator>();
    }

    public void Reset(bool anim=false)
    {
        if (anim)
        {
            StartCoroutine(nameof(ResetAfterAnimation));
        } else
        {
            EnteredPassword = "";
            UpdateText();
        }
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
        if (EnteredPassword.Length >= Password.Length) Reset(true);

        if (Reverse)
        {
            EnteredPassword = c + EnteredPassword;
        } else
        {
            EnteredPassword += c;
        }

        if (EnteredPassword.Length == Password.Length)
        {
            if (EnteredPassword == Password)
            {
                OnPasswordCorrect.Invoke();
            } else
            {
                Reset(true);
            }
        }

        UpdateText();
    }

    public void UpdateText()
    {
        string s = "";
        for(int i=0; i<Password.Length; i++)
        {
            s += EnteredPassword.Length > i ? EnteredPassword[i] : PlaceHolder;
        }

        EnteredPasswordText.text = s;
    }
}
