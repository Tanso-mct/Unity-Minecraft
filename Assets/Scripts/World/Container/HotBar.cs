using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotBar : Container
{
    private int selectingSlot = 1;
    public int SelectingSlot{ get { return selectingSlot; } }

    [SerializeField] private GameObject selectFrame;
    [SerializeField] private float selectFrameMoveVal = 98.0f;

    [SerializeField] private Container inventory;

    [SerializeField] private GameObject holdingBlock;
    [SerializeField] private GameObject holdingItemParent;

    [SerializeField] private GameObject holdingBlockIdle;
    [SerializeField] private GameObject holdingItemParentIdle;

    [SerializeField] private Texture holdingBlockTexture;

    public override void Init()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].Init(i+1);
        }

        holdingBlock.SetActive(false);
        holdingBlockIdle.SetActive(false);

        holdingItemParent.SetActive(false);
        holdingItemParentIdle.SetActive(false);
    }

    private void CreateHolding(int vaxelId)
    {
        if (vaxelId == 0) return;

        holdingBlock.SetActive(false);
        holdingBlockIdle.SetActive(false);

        holdingItemParent.SetActive(false);
        holdingItemParentIdle.SetActive(false);

        switch (vaxelId)
        {
            case (int)Constants.VAXEL_TYPE.DIRT:
                holdingBlock.SetActive(true);
                holdingBlockIdle.SetActive(true);
                SupportFunc.LoadTexture(ref holdingBlockTexture, Constants.SPRITE_DIRT);

                holdingBlock.GetComponent<MeshRenderer>().material.mainTexture = holdingBlockTexture;
                holdingBlockIdle.GetComponent<MeshRenderer>().material.mainTexture = holdingBlockTexture;
                return;

            case (int)Constants.VAXEL_TYPE.GRASS_TOP:
                holdingBlock.SetActive(true);
                holdingBlockIdle.SetActive(true);
                SupportFunc.LoadTexture(ref holdingBlockTexture, Constants.SPRITE_GRASS_TOP);

                holdingBlock.GetComponent<MeshRenderer>().material.mainTexture = holdingBlockTexture;
                holdingBlockIdle.GetComponent<MeshRenderer>().material.mainTexture = holdingBlockTexture;
                return;
        }
    }

    public void UpdateHolding()
    {
        CreateHolding(GetIsContain(selectingSlot));
    }

    public void SelectSlot(int slot)
    {
        if (!(slot >= 1 && slot <= 9)) return;

        selectFrame.transform.position = new Vector3
        (
            selectFrame.transform.position.x + (slot - selectingSlot) * selectFrameMoveVal,
            selectFrame.transform.position.y,
            selectFrame.transform.position.z
        );

        selectingSlot = slot;

        CreateHolding(GetIsContain(slot));
    }

    public override bool AddContent(int vaxelId)
    {
        int nowVaxelId = 0;
        int nowAmount = 0;
        for (int i = 0; i < slots.Count; i++)
        {
            bool isStackable = slots[i].GetIsContain(ref nowVaxelId, ref nowAmount);
            if (nowVaxelId == 0) // 空いているスロット
            {
                slots[i].SetContents(vaxelId, 1);
                inventory.slots[i].SetContents(vaxelId, 1);
                return true;
            }
            else if (isStackable && nowVaxelId == vaxelId && nowAmount < stackMax) 
            {
                slots[i].SetContents(vaxelId, nowAmount + 1);
                inventory.slots[i].SetContents(vaxelId, nowAmount + 1);
                return true;
            }
        }

        // 空いているスロットがないため、何もしない
        return false;
    }

    public override Vector2 RemoveContent(int amount, int slot) // return x: vaxelId, y: amount
    {
        int nowVaxelId = 0;
        int nowAmount = 0;
        bool isStackable = slots[slot-1].GetIsContain(ref nowVaxelId, ref nowAmount);

        if (isStackable && nowVaxelId != 0 && nowAmount > amount)
        {
            slots[slot-1].SetContents(nowVaxelId, nowAmount - amount);
            inventory.slots[slot-1].SetContents(nowVaxelId, nowAmount - amount);
            return new Vector2(nowVaxelId, amount);
        }
        else if (isStackable && nowVaxelId != 0 && nowAmount <= amount)
        {
            slots[slot-1].SetContents(0, 0);
            inventory.slots[slot-1].SetContents(0, 0);
            return new Vector2(nowVaxelId, nowAmount);
        }
        else if (!isStackable && nowVaxelId != 0)
        {
            slots[slot-1].SetContents(0, 0);
            inventory.slots[slot-1].SetContents(0, 0);
            return new Vector2(nowVaxelId, 1);
        }
        else
        {
            return new Vector2(0, 0);
        }
    }
}
