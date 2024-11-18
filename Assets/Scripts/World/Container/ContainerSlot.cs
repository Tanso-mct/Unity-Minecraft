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
        SupportFunc.LoadSprite(ref nullItemSprite, Constants.SPRITE_NULL_ITEM);

        itemImage.sprite = nullItemSprite;

        textEl.initText.text = "";

        id = (int)Constants.VAXEL_TYPE.AIR;
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

    public void GetIsContain(ref int vaxelId, ref int amount)
    {
        vaxelId = id;
        amount = this.amount;
    }

    private string GetSpritePath()
    {
        switch (id)
        {
            case (int)Constants.VAXEL_TYPE.AIR:
                return Constants.SPRITE_NULL_ITEM;
        }

        return "";
    }

    public void SetItem(int vaxelId)
    {
        id = vaxelId;
        string spritePath = GetSpritePath();

        Sprite itemSprite = null;
        SupportFunc.LoadSprite(ref itemSprite, spritePath);

        itemImage.sprite = itemSprite;
        itemImage.transform.localScale = new Vector3(itemScale, itemScale, itemScale);

        textEl.initText.text = "";
        amount = 0;
    }

    public void SetBlock(int vaxelId, int amount)
    {
        id = vaxelId;
        string spritePath = GetSpritePath();

        Sprite blockSprite = null;
        SupportFunc.LoadSprite(ref blockSprite, spritePath);

        itemImage.sprite = blockSprite;
        itemImage.transform.localScale = new Vector3(blockScale, blockScale, blockScale);

        textEl.initText.text = amount.ToString();
        this.amount = amount;
    }
}
