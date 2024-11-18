using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Container : MonoBehaviour
{
    private List<Vaxel> items = new List<Vaxel>();
    [SerializeField] private List<ContainerSlot> slots;


    private int width = 9;
    [SerializeField] private int height = 4;

    private int stackMax = 64;

    public void Init()
    {
        items = new List<Vaxel>();
    }

    public int GetIsContain(int slot)
    {
        return slots[slot-1].GetIsContain();
    }

    public bool AddContent(int vaxelId)
    {
        int nowVaxelId = 0;
        int nowAmount = 0;
        for (int i = 0; i < slots.Count; i++)
        {
            bool isStackable = slots[i].GetIsContain(ref nowVaxelId, ref nowAmount);
            if (nowVaxelId == 0) // 空いているスロット
            {
                slots[i].SetContents(vaxelId, 1);
                return true;
            }
            else if (isStackable && nowVaxelId == vaxelId && nowAmount < stackMax) 
            {
                slots[i].SetContents(vaxelId, nowAmount + 1);
                return true;
            }
        }

        // 空いているスロットがないため、何もしない
        return false;
    }

    public Vector2 RemoveContent(int amount, int slot) // return x: vaxelId, y: amount
    {
        int nowVaxelId = 0;
        int nowAmount = 0;
        bool isStackable = slots[slot-1].GetIsContain(ref nowVaxelId, ref nowAmount);

        if (isStackable && nowVaxelId != 0 && nowAmount > amount)
        {
            slots[slot-1].SetContents(nowVaxelId, nowAmount - amount);
            return new Vector2(nowVaxelId, amount);
        }
        else if (isStackable && nowVaxelId != 0 && nowAmount <= amount)
        {
            slots[slot-1].SetContents(0, 0);
            return new Vector2(nowVaxelId, nowAmount);
        }
        else if (!isStackable && nowVaxelId != 0)
        {
            slots[slot-1].SetContents(0, 0);
            return new Vector2(nowVaxelId, 1);
        }
        else
        {
            return new Vector2(0, 0);
        }
    }
}
