using UnityEngine;
using UnityEngine.EventSystems;

public class BlockTray : MonoBehaviour, IDropHandler, IPointerExitHandler
{
    public VPLZone zone;
    private GameObject preview;
    public bool IsRoot = false;

    public void Execute()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var tr = transform.GetChild(i);
            if (tr.TryGetComponent<BaseBlock>(out var block))
            {
                block.Execute();
            }
        }
    }

    public void UpdateGhostPosition(GameObject block, Vector2 pointerPosition)
    {
        if (!enabled) return;

        int newIndex = 0;

        if (block.GetComponent<BaseBlock>().hasTopPort)
        {
            if (preview == null) {
                SpawnGhost(block);
            }

            for (int i = transform.childCount-1; i >= 0; i--)
            {
                // Skip the ghost itself in the calculation
                var obj = transform.GetChild(i);
                if (obj.gameObject == preview) continue;

                if (pointerPosition.y < obj.position.y)
                {
                    var currIndex = preview.transform.GetSiblingIndex();
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
        } else if (preview == null) // Edge case in which an event isn't present in the tray
        {
            SpawnGhost(block);
        }

        preview.transform.SetSiblingIndex(newIndex);
    }

    public void SpawnGhost(GameObject block)
    {
        preview = Instantiate(block, transform);
        var group = preview.GetComponent<CanvasGroup>();
        group.alpha = 0.4f;
        group.blocksRaycasts = false;
        preview.GetComponent<BaseBlock>().isStatic = true;
    }

    public void DestroyPreview()
    {
        // Remove ghost when mouse leaves the tray area
        if (preview != null) Destroy(preview);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DestroyPreview();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && preview != null)
        {
            // Snap the block into the Tray at the ghost's position
            eventData.pointerDrag.transform.SetParent(transform);
            eventData.pointerDrag.GetComponent<BaseBlock>().Activated(zone);
            eventData.pointerDrag.transform.SetSiblingIndex(preview.transform.GetSiblingIndex());
            Destroy(preview);
        }
    }
}