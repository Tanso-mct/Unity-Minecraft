using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockAdmin : MonoBehaviour
{
    // ワールド上の何らかのデータを持つブロックを管理
    private Dictionary<Vector3Int, Vaxel> dataContainBlocks;

    private Vaxel setVaxel;
    private Vaxel useVaxel;
    private Vaxel breakVaxel;

    [SerializeField] private GameObject entityItemParent;

    public void Init()
    {
        setVaxel = null;
        useVaxel = null;
        breakVaxel = null;
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
                rtVaxel.Init(entityItemParent);
                break;
        }

        return rtVaxel;
    }

    private Vaxel GetVaxel(Vector4 block)
    {
        int blockId = (int)block.w;
        return GetVaxelInstance(blockId);
    }

    private Vaxel GetVaxel(Container container, int slot)
    {
        int blockId = (int)Constants.VAXEL_TYPE.DIRT;
        return GetVaxelInstance(blockId);
    }

    public bool IsUseable(int blockId)
    {
        // switch (blockId)
        // {
        //     case (int)Constants.VAXEL_TYPE.:
        //         return true;

            
        // }

        return false;
    }

    public void Set(Vector4 block, ref Vector4 frameSetBlock, Container sourceContainer, int setSlot)
    {
        setVaxel = GetVaxel(sourceContainer, setSlot);
        setVaxel.TrySet(block, ref frameSetBlock, sourceContainer, setSlot);
    }

    public void Use(Vector4 block, Container sourceContainer)
    {
        useVaxel = GetVaxel(block);
        useVaxel.UseBlock(block, sourceContainer);
    }

    public void Break(Vector4 block, ref Vector4 frameDestroyBlock)
    {
        breakVaxel = GetVaxel(block);
        breakVaxel.TryBreak(block, ref frameDestroyBlock);
    }

    public bool FinishedSet(Vector4 frameSetBlock)
    {
        if (setVaxel == null) return false;

        setVaxel.FinishedSet(frameSetBlock);
        setVaxel = null;

        return true;
    }

    public bool FinishedBreak(Vector4 frameDestroyBlock)
    {
        if (breakVaxel == null) return false;

        breakVaxel.FinishedBreak(frameDestroyBlock);
        breakVaxel = null;
        return true;
    }



}
