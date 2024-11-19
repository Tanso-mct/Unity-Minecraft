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
