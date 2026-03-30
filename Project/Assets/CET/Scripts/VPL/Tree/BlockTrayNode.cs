using System;
using System.Collections.Generic;

[Serializable]
public class BlockTrayNode
{
    public BlockTrayNode()
    {
        Ident = Guid.NewGuid().ToString();
    }

    public string Ident;

    public List<BlockNode> Blocks = new();
}
