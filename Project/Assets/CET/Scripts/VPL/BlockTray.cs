using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockTray : MonoBehaviour, IDropHandler, IPointerExitHandler
{
    public VPLZone Zone;
    private GameObject preview;
    public bool IsRoot = false;

    public List<BaseBlock> Blocks => Helpers.GetComponentsInChildren<BaseBlock>(transform);

    public BlockTrayNode SaveNode()
    {
        List<BlockNode> nodes = new();

        foreach (var block in Blocks)
        {
            nodes.Add(block.SaveNode());
        }

        return new()
        {
            Blocks = nodes
        };
    }

    public void LoadNode(BlockTrayNode node)
    {
        foreach (var blockNode in node.Blocks)
        {
            var def = Zone.Store.GetDefinitionByName(blockNode.DefinitionName);
            var blockPrefab = Zone.Store.GetPrefabForDefinition(def);
            if (blockPrefab != null)
            {
                var spawned = Instantiate(blockPrefab, transform);
                spawned.Activated(Zone);
                spawned.LoadNode(blockNode);
            } else
            {
                print($"Couldn't find prefab of {def.Name}: {def.PrefabName}");
            }
        }
    }

    public void Activated(VPLZone zone)
    {
        enabled = true;
        Zone = zone;
    }

    public IEnumerator Execute()
    {
       for (int i = 0; i < transform.childCount; i++)  {
            var tr = transform.GetChild(i);
            if (tr.TryGetComponent<BaseBlock>(out var block) && !block.IsExpression)
            {
                yield return block.Execute();
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
        preview = Instantiate(block, transform, true);
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
            var block = eventData.pointerDrag;
            // Snap the block into the Tray at the ghost's position
            block.transform.SetParent(transform);
            block.GetComponent<BaseBlock>().Activated(Zone);
            block.transform.SetSiblingIndex(preview.transform.GetSiblingIndex());
            Destroy(preview);
        }
    }
}