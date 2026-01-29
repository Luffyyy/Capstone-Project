using UnityEngine;

public class Dialog : MonoBehaviour
{
    public void Show()
    {
        gameObject.SetActive(true);       
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
