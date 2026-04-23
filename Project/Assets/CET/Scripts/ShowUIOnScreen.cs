using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShowUIOnScreen : Interactable
{
    public string TextToShow;
    public Texture Texture;
    public Vector3 Rotation = Vector3.zero;
    public Vector2 Size = new Vector2(800, 1700);
    public Vector2 ButtonPosition = new Vector2(414, 164);

    void Start()
    {
    }

    void Update()
    {
    }

    public override void Interact()
    {
        if (ShowUIOnScreenManager.Instance != null)
        {
            
            ShowUIOnScreenManager.Instance.ShowUI(Texture, Rotation, Size, ButtonPosition, TextToShow);
        }
    }
}
