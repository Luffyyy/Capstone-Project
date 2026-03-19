using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseBlock : MonoBehaviour
{
    [HideInInspector] // Set by VPLState itself
    public VPLZone Zone;

    public string Name;
    public Color Color;

    protected TextMeshProUGUI NameText;

    protected BaseBlock NextBlock;

    // [HideInInspector]
    public bool isStatic = false;

    // Events have no top port, they self initiate, for example.
    public bool hasTopPort = true;
    public bool hasBottomPort = true;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    void Awake()
    {
        SetName(Name);
        SetColor(Color);
    }

    // Called when the block is freshly spawned
    public virtual void Activated(VPLZone zone)
    {
        Zone = zone;
        GetComponent<DraggableBlock>().IsNew = false;
    }

    public virtual void SetName(string name)
    {
        Name = name;
        NameText = transform.Find("Name").GetComponent<TextMeshProUGUI>();
        NameText.SetText(Name);
    }

    public virtual void SetColor(Color color)
    {
        Color = color;
        if (Color != null)
        {
            GetComponent<Image>().color = color;
        }
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
