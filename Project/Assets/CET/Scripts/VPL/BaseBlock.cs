using System.Collections;
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

    [HideInInspector]
    public bool isStatic = false;

    public BlockDefinition Defintion;

    public virtual void Awake()
    {
        SetName(Name);
        SetColor(Color);
    }

    public virtual void OnDelete()
    {
        
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
}
