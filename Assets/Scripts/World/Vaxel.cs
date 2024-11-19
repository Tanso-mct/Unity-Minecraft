using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

abstract public class Vaxel
{
    protected int id;

    protected int setSlot;

    protected int setBlockId;
    protected Container sourceSetContainer;

    protected int breakBlockId;
    protected Container sourceBreakContainer;

    public virtual void Init()
    {
        setBlockId = (int)Constants.VAXEL_TYPE.AIR;
        breakBlockId = (int)Constants.VAXEL_TYPE.AIR;
    }

    abstract public void LoadFromJson(ref WorldData worldData);
    abstract public void SaveToJson(ref WorldData worldData);

    abstract public float GetDurability();

    abstract public float GetMiningSpeed(Constants.VAXEL_TYPE blockType);

    public virtual bool IsUseable()
    {
        return false;
    }

    public virtual void UseBlock(Vector4 block, Container sourceContainer)
    {

    }

    public virtual void UseItem(Camera cam, Vector4 target, ref Vector4 frameSetBlock)
    {

    }

    public virtual void FinishedSet(Vector4 frameSetBlock)
    {
        // Debug.Log("=====================================");
        // Debug.Log("Finished Set Block : " + setBlockId);
        // Debug.Log("Before Block : " + frameSetBlock.w);
        // Debug.Log("=====================================");

        HotBar hotBar = sourceSetContainer as HotBar;
        hotBar.RemoveContent(1, hotBar.SelectingSlot);
    }

    public virtual void FinishedBreak(Vector4 frameDestroyBlock)
    {
        // Debug.Log("=====================================");
        // Debug.Log("Finished Break Block : " + breakBlockId);
        // Debug.Log("Before Block : " + frameDestroyBlock.w);
        // Debug.Log("=====================================");

        sourceBreakContainer.AddContent(breakBlockId);
    }

    public virtual void TryBreak(Vector4 block, ref Vector4 frameDestroyBlock, Container sourceContainer)
    {
        sourceBreakContainer = sourceContainer;
        frameDestroyBlock = block;
        breakBlockId = (int)block.w;
    }

    public virtual void TrySet(Vector4 block, ref Vector4 frameSetBlock, Container sourceContainer, int slot)
    {
        setSlot = slot;
        sourceSetContainer = sourceContainer;

        setBlockId = sourceContainer.GetIsContain(slot);

        Vector4 setBlock = block;
        setBlock.w = setBlockId;

        frameSetBlock = setBlock;
    }

    public void DiscardItem(Vector3 playerPos, Vector3 playerDir)
    {

    }
}
