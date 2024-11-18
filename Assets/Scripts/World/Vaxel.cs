using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Vaxel
{
    protected int id;

    protected int setSlot;
    protected Container sourceSetContainer;

    protected Container sourceBreakContainer;

    public virtual void Init()
    {

    }

    abstract public void LoadFromJson(ref WorldData worldData);
    abstract public void SaveToJson(ref WorldData worldData);

    abstract public float GetDurability();

    abstract public float GetMiningSpeed(Constants.VAXEL_TYPE blockType);

    public virtual bool IsUseable()
    {
        return false;
    }

    public virtual void UseBlock(Vector4 block, Container sourceContainer, int setSlot)
    {

    }

    public virtual void UseItem(Camera cam, Vector4 target, ref Vector4 frameSetBlock)
    {

    }

    public virtual void FinishedSet(Vector4 block, ref Vector4 frameSetBlock)
    {

    }

    public virtual void FinishedBreak(Vector4 block, ref Vector4 frameDestroyBlock)
    {

    }

    public virtual void TryBreak(Vector4 block, ref Vector4 frameDestroyBlock, Container sourceContainer)
    {
        sourceBreakContainer = sourceContainer;
        frameDestroyBlock = block;
    }

    public virtual void TrySet(Vector4 block, ref Vector4 frameSetBlock, Container sourceContainer, int slot)
    {
        setSlot = slot;
        sourceSetContainer = sourceContainer;

        Vector4 setBlock = block;
        setBlock.w = (float)Constants.VAXEL_TYPE.DIRT;;

        frameSetBlock = setBlock;
    }

    public void DiscardItem(Vector3 playerPos, Vector3 playerDir)
    {

    }
}
