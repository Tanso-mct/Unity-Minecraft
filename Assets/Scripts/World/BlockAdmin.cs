using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockAdmin : MonoBehaviour
{
    // ワールド上の何らかのデータを持つブロックを管理
    private Dictionary<Vector3Int, Vaxel> dataContainBlocks;

    public void Init()
    {

    }

    private Vaxel GetVaxelInstance(int blockId)
    {
        Vaxel rtVaxel = null;

        switch (blockId)
        {
            case (int)Constants.VAXEL_TYPE.BEDROCK:
            case (int)Constants.VAXEL_TYPE.DIRT:
            case (int)Constants.VAXEL_TYPE.GRASS_TOP:
            case (int)Constants.VAXEL_TYPE.OBSIDIAN:
                rtVaxel = new BuildBlockVaxel();
                rtVaxel.Init();
                break;
        }

        return rtVaxel;
    }

    private Vaxel GetVaxel(Vector4 block)
    {
        int blockId = (int)block.w;
        return GetVaxelInstance(blockId);
    }

    private Vaxel GetVaxel()
    {
        int blockId = (int)Constants.VAXEL_TYPE.DIRT;
        return GetVaxelInstance(blockId);
    }

    public bool IsUseable(int blockId)
    {
        return false;
    }

    public void Set(Vector4 block, ref Vector4 frameSetBlock, Container sourceContainer, int setSlot)
    {
        Vaxel vaxel = GetVaxel();
        vaxel.TrySet(block, ref frameSetBlock, sourceContainer, setSlot);
    }

    public void Use(Vector4 block, Container sourceContainer, int setSlot)
    {
        Vaxel vaxel = GetVaxel(block);
        vaxel.UseBlock(block, sourceContainer, setSlot);
    }

    public void Break(Vector4 block, ref Vector4 frameDestroyBlock, Container sourceContainer)
    {
        Vaxel vaxel = GetVaxel(block);
        vaxel.TryBreak(block, ref frameDestroyBlock, sourceContainer);
    }


}
