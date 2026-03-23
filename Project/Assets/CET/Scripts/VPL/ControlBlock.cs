using System.Collections;
using System.Collections.Generic;

public class ControlBlock : BaseBlock
{
    public List<BaseBlock> Children;

    public override IEnumerator Execute()
    {
        foreach (var block in Children)
        {
            block.Execute();
        }

        yield return null;
    }
}
