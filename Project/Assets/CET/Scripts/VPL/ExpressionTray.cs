using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Outline))]
public class ExpressionTray : MonoBehaviour, IDropHandler, IPointerExitHandler, IPointerEnterHandler
{
    [HideInInspector]
    public VPLZone Zone;
    public BaseBlock CurrentExpression;

    public BaseBlock DefaultBlock;

    public bool IsActivated = false;

    public ExpressionTrayNode SaveNode()
    {
        return new()
        {
            CurrentExpression = CurrentExpression != null ? CurrentExpression.SaveNode() : null
        };
    }

    public void LoadNode(ExpressionTrayNode node)
    {
        var blockNode = node.CurrentExpression;
        if (blockNode != null)
        {
            var def = Zone.Store.GetDefinitionByName(blockNode.DefinitionName);
            if (def != null)
            {
                var blockPrefab = Zone.Store.GetPrefabForDefinition(def);
                if (blockPrefab is BaseBlock block && block.IsExpression)
                {
                    var spawned = Instantiate(block, transform);
                    SetCurrentExpression(spawned);
                    spawned.LoadNode(blockNode);
                } else
                {
                    print($"Couldn't find prefab of {def.Name}: {def.PrefabName}");
                }
            }
        }

        CheckIfToPlaceDefaultExpression();
    }

    public void Activated(VPLZone zone)
    {
        Zone = zone;
        IsActivated = true;
        if (CurrentExpression != null)
        {
            CurrentExpression.gameObject.SetActive(true);
            CurrentExpression.Activated(zone);
        }
        if (DefaultBlock != null)
        {
            DefaultBlock.Activated(zone);
        }
        CheckIfToPlaceDefaultExpression();
    }

    public object Evaluate()
    {
        if (CurrentExpression != null)
        {
            return CurrentExpression.Evaluate();
        } else
        {
            return null;            
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsActivated && eventData.pointerDrag != null && eventData.pointerDrag.TryGetComponent<BaseBlock>(out var block) && block.IsExpression)
        {
            GetComponent<Outline>().enabled = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (IsActivated && eventData.pointerDrag != null && eventData.pointerDrag.TryGetComponent<BaseBlock>(out var block) && block.IsExpression)
        {
            GetComponent<Outline>().enabled = false;
        }
    }

    private void CheckIfToPlaceDefaultExpression()
    {
        if (CurrentExpression == null && DefaultBlock != null)
        {
            CurrentExpression = DefaultBlock;
            CurrentExpression.GetComponent<DraggableBlock>().enabled = false; // Don't allow dragging it out
            CurrentExpression.gameObject.SetActive(IsActivated);
        }
    }

    public void SetCurrentExpression(BaseBlock exp)
    {
        if (CurrentExpression != null && CurrentExpression != exp)
        {
            if (CurrentExpression == DefaultBlock)
            {
                CurrentExpression.gameObject.SetActive(false);
            } else
            {
                Destroy(CurrentExpression.gameObject); // Only allow a single expression at a time
            }
        }

        exp.transform.SetParent(transform, false);
        exp.Activated(Zone);
        GetComponent<Outline>().enabled = false;

        CurrentExpression = exp;
        CheckIfToPlaceDefaultExpression();
    }

    public void RemoveCurrentExpression()
    {
        CurrentExpression = null;
        GetComponent<Outline>().enabled = false;
        CheckIfToPlaceDefaultExpression();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (IsActivated && eventData.pointerDrag.TryGetComponent<BaseBlock>(out var block) && block.IsExpression)
        {
            SetCurrentExpression(block);
        }
    }
}
