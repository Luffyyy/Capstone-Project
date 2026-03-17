using TMPro;
using UnityEngine;

public class BaseBlock : MonoBehaviour
{
    public VPLState state;

    public string Name;

    protected TextMeshProUGUI NameText;

    protected BaseBlock NextBlock;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        NameText = transform.Find("Name").GetComponent<TextMeshProUGUI>();
        NameText.SetText(Name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // Executes the block
    public virtual void Execute()
    {
        
    }
}
