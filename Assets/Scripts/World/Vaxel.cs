using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

abstract public class Vaxel
{
    protected int id;
    public int ID { get { return id; } }

    protected int setSlot;

    protected int setBlockId;
    protected Container sourceSetContainer;

    protected int breakBlockId;

    protected GameObject entityItemParent;

    private float entityItemOffY = 0.7f;

    public virtual void Init(GameObject entityItemParent)
    {
        setBlockId = (int)Constants.VAXEL_TYPE.AIR;
        breakBlockId = (int)Constants.VAXEL_TYPE.AIR;

        this.entityItemParent = entityItemParent;
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

        GameObject entityItem = null;
        GameObject entityBlockPrefab = null;
        SupportFunc.InstantiatePrefab
        (
            ref entityItem, ref entityBlockPrefab, Constants.PREFAB_ENTITY_BLOCK, ref entityItemParent, 
            SupportFunc.PosFloatConvert(new Vector3(frameDestroyBlock.x, frameDestroyBlock.y + entityItemOffY, frameDestroyBlock.z))
        );

        EntityItem thisItem = entityItem.GetComponent<EntityItem>();
        thisItem.ThrowIt(new Vector3(Random.Range(0, 2), Random.Range(0, 2), Random.Range(0, 2)), breakBlockId, SupportFunc.LoadMultiTextureFromId(breakBlockId));
    }

    public virtual void TryBreak(Vector4 block, ref Vector4 frameDestroyBlock)
    {
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
