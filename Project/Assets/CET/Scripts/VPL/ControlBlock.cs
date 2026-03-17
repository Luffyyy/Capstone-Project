using System.Collections.Generic;
using UnityEngine;

public class ControlBlock : BaseBlock
{
    public List<BaseBlock> Children;

    public override void Execute()
    {
        foreach (var block in Children)
        {
            block.Execute();
        }
    }
}
