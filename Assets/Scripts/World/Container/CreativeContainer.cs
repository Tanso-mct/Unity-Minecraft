using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreativeContainer : Container
{
    [SerializeField] private Container hotBar;
    [SerializeField] private Container inventory;

    public override void Init()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].Init(i+1);
        }

        for (int i = 9; i < slots.Count; i++)
        {
            slots[i].SetContents((int)Constants.VAXEL_TYPE.STONE, 1);
        }
    }

    protected override void SetSlotContent(int slotId, int vaxelId, int amount)
    {
        if (slotId-1 < hotBar.slots.Count)
        {
            slots[slotId-1].SetContents(vaxelId, amount);
            hotBar.slots[slotId-1].SetContents(vaxelId, amount);
            inventory.slots[slotId-1].SetContents(vaxelId, amount);
        }
    }

    public override void DropItem(int slotId)
    {
        if (slotId >= 1 && slotId <= 9)
        {
            if (McControls.IsKeyDown(Constants.CONTROL_DROP_ITEM) && !Input.GetKey(KeyCode.LeftShift))
            {
                RemoveContent(1, slotId);
            }
            else if (McControls.IsKeyDown(Constants.CONTROL_DROP_ITEM) && Input.GetKey(KeyCode.LeftShift))
            {
                RemoveContent(64, slotId);
            }
        }
    }

    public override void SlotQuickMove()
    {
        int startSlotVaxelId = 0;
        int startSlotAmount = 0;
        bool startSlotIsStackable = slots[startSlotId-1].GetIsContain(ref startSlotVaxelId, ref startSlotAmount);

        if (startSlotId >= 1 && startSlotId <= 9)
        {
            SetSlotContent(startSlotId, 0, 0);
        }
        else
        {
            int thisSlotVaxelId = 0;
            int thisSlotAmount = 0;
            bool thisSlotIsStackable = false;
            for (int i = 0; i < 9; i++)
            {
                if (!slots[i].isQuickMoveable) continue;

                thisSlotIsStackable = slots[i].GetIsContain(ref thisSlotVaxelId, ref thisSlotAmount);
                if (thisSlotVaxelId == 0)
                {
                    SetSlotContent(i+1, startSlotVaxelId, stackMax);
                    return;
                }
                else if (thisSlotIsStackable && thisSlotVaxelId == startSlotVaxelId && thisSlotAmount != stackMax)
                {
                    SetSlotContent(i+1, startSlotVaxelId, stackMax);
                    return;
                }
            }
        }
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
                if (i < hotBar.slots.Count) hotBar.slots[i].SetContents(vaxelId, 1);
                return true;
            }
            else if (isStackable && nowVaxelId == vaxelId && nowAmount < stackMax) 
            {
                slots[i].SetContents(vaxelId, nowAmount + 1);
                if (i < hotBar.slots.Count) hotBar.slots[i].SetContents(vaxelId, nowAmount + 1);
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
            if (slot <= hotBar.slots.Count) hotBar.slots[slot-1].SetContents(nowVaxelId, nowAmount - amount);
            return new Vector2(nowVaxelId, amount);
        }
        else if (isStackable && nowVaxelId != 0 && nowAmount <= amount)
        {
            slots[slot-1].SetContents(0, 0);
            if (slot <= hotBar.slots.Count) hotBar.slots[slot-1].SetContents(0, 0);
            return new Vector2(nowVaxelId, nowAmount);
        }
        else if (!isStackable && nowVaxelId != 0)
        {
            slots[slot-1].SetContents(0, 0);
            if (slot <= hotBar.slots.Count) hotBar.slots[slot-1].SetContents(0, 0);
            return new Vector2(nowVaxelId, 1);
        }
        else
        {
            return new Vector2(0, 0);
        }
    }
}
