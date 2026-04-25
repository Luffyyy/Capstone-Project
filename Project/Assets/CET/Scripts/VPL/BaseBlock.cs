using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseBlock : MonoBehaviour
{
    public const float BLOCK_SCALE = 1.25f;

    [HideInInspector] // Set by VPLState itself
    public VPLZone Zone;

    public string Name;
    public Color Color;

    protected TextMeshProUGUI NameText;

    [HideInInspector]
    public bool isStatic = false;

    public BlockDefinition Defintion;


    // Events have no top port, they self initiate, for example.
    public bool hasTopPort = true;
    public bool hasBottomPort = true;

    public bool IsExpression = false;

    public virtual BlockNode SaveNode()
    {
        return new()
        {
            DefinitionName = Defintion.name,
        };
    }

    public virtual void LoadNode(BlockNode node)
    {
        SetDefinitionByName(node.DefinitionName);
    }

    public virtual void Awake()
    {
        SetName(Name);
        SetColor(Color);
    }

    public virtual void OnDelete()
    {
        
    }

    public void SetDefinitionByName(string name)
    {
        var def = Zone.Store.GetDefinitionByName(name);
        if (def != null)
        {
            SetDefinition(def);
        }
    }

    public virtual void SetDefinition(BlockDefinition def)
    {
        Defintion = def;
        if (def.Name != null)
        {
            SetName(def.Name);
            SetColor(def.Color);
        }
    }

    // Called when the block is freshly spawned
    public virtual void Activated(VPLZone zone)
    {
        Zone = zone;
        GetComponent<DraggableBlock>().IsNew = false;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public virtual void SetName(string name)
    {
        var nameObject = transform.Find("Name");
        if (nameObject != null)
        {
            Name = name;
            NameText = nameObject.GetComponent<TextMeshProUGUI>();
            NameText.SetText(Name);
        }
    
    }

    public virtual void SetColor(Color color)
    {
        // Temporarily turned off
        // Color = color;
        // if (Color != null)
        // {
        //     GetComponent<Image>().color = color;
        // }
    }

    public virtual object Evaluate()
    {
        return null;
    }

    // Executes the block
    public virtual IEnumerator Execute()
    {
        yield return null;
    }
}
