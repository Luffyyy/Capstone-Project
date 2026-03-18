using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BlockTray : MonoBehaviour, IDropHandler, IPointerExitHandler
{
    private GameObject ghostPreview;

    public void UpdateGhostPosition(GameObject block, Vector2 pointerPosition)
    {


        int newIndex = 0;

        if (block.GetComponent<BaseBlock>().hasTopPort)
        {
            if (ghostPreview == null) {
                SpawnGhost(block);
            }

            for (int i = transform.childCount-1; i >= 0; i--)
            {
                // Skip the ghost itself in the calculation
                var obj = transform.GetChild(i);
                if (obj.gameObject == ghostPreview) continue;

                if (pointerPosition.y < obj.position.y)
                {
                    var currIndex = ghostPreview.transform.GetSiblingIndex();
                    if (currIndex < i)
                    {
                        newIndex = i;
                    } else
                    {
                        newIndex = i+1;
                    }

                    break;
                }
            }
        }
        
        if (newIndex == 0 && !transform.GetChild(0).GetComponent<BaseBlock>().hasTopPort)
        {
            return;
        } else if (ghostPreview == null) // Edge case in which an event isn't present in the tray
        {
            SpawnGhost(block);
        }

        ghostPreview.transform.SetSiblingIndex(newIndex);
    }

    public void SpawnGhost(GameObject block)
    {
        ghostPreview = Instantiate(block, transform);
        var group = ghostPreview.GetComponent<CanvasGroup>();
        group.alpha = 0.4f;
        group.blocksRaycasts = false;
        ghostPreview.GetComponent<BaseBlock>().isStatic = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Remove ghost when mouse leaves the tray area
        if (ghostPreview != null) Destroy(ghostPreview);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && ghostPreview != null)
        {
            // Snap the block into the Tray at the ghost's position
            eventData.pointerDrag.transform.SetParent(transform);
            eventData.pointerDrag.GetComponent<DraggableBlock>().IsNew = false;
            eventData.pointerDrag.transform.SetSiblingIndex(ghostPreview.transform.GetSiblingIndex());
            Destroy(ghostPreview);
        }
    }
}