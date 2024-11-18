using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

using UnityEngine.UI;

public class ContainerSlot : MonoBehaviour
{
    [SerializeField] private Container container;
    [SerializeField] private TextEl textEl;

    [SerializeField] private Image selector;
    [SerializeField] private Image itemImage;

    [SerializeField] private float itemScale = 0.5f;
    [SerializeField] private float blockScale = 0.5f;

    private int id;
    private int amount = 0;

    public void Init()
    {
        Sprite nullItemSprite = null;
        SupportFunc.LoadSprite(ref nullItemSprite, Constants.SPRITE_NULL);

        itemImage.sprite = nullItemSprite;

        textEl.initText.text = "";

        id = 0;
        amount = 0;
    }

    public void OnHover()
    {
        selector.gameObject.SetActive(true);
    }

    public void UnHover()
    {
        selector.gameObject.SetActive(false);
    }

    public void OnClick()
    {
        Debug.Log("Slot Clicked");
    }

    public bool GetIsContain(ref int vaxelId, ref int amount)
    {
        vaxelId = id;
        amount = this.amount;

        return IsStackable(vaxelId);
    }

    public int GetIsContain()
    {
        return id;
    }

    private string GetSpritePath()
    {
        switch (id)
        {
            case (int)Constants.VAXEL_TYPE.AIR:
                return Constants.SPRITE_NULL;

            case (int)Constants.VAXEL_TYPE.DIRT:
                return Constants.SPRITE_DIRT;

            case (int)Constants.VAXEL_TYPE.GRASS_TOP:
                return Constants.SPRITE_GRASS_SIDE;
        }

        return "";
    }

    private bool IsStackable(int vaxelId)
    {
        // switch (vaxelId)
        // {
        //     case (int)Constants.VAXEL_TYPE.:
        //         return true;
        // }

        return true;
    }

    private bool IsItem(int vaxelId)
    {
        // switch (vaxelId)
        // {
        //     case (int)Constants.VAXEL_TYPE.:
        //         return true;
        // }

        return false;
    }

    public void SetContents(int vaxelId, int amount)
    {
        id = vaxelId;
        string spritePath = GetSpritePath();

        Sprite blockSprite = null;
        SupportFunc.LoadSprite(ref blockSprite, spritePath);

        itemImage.sprite = blockSprite;

        if (IsItem(vaxelId))
        {
            itemImage.transform.localScale = new Vector3(itemScale, itemScale, itemScale);
        }
        else
        {
            itemImage.transform.localScale = new Vector3(blockScale, blockScale, blockScale);
        }

        if (amount != 0)
        {
            textEl.initText.text = amount.ToString();
        }
        else
        {
            textEl.initText.text = "";
        }

        this.amount = amount;
    }
}
