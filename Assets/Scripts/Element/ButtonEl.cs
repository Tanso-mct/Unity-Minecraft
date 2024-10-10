using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEl : Element
{
    public override void Init()
    {
        Debug.Log("Init ButtonEl [" + name + "]");

        // 画像の初期化処理
        InitImages();
    }

    public override void Show()
    {
        Debug.Log("Show ButtonEl [" + name + "]");

        // 画像の表示処理
        ShowImages(true);
    }

    public override void Close()
    {
        Debug.Log("Close ButtonEl [" + name + "]");

        // 画像の非表示処理
        ShowImages(false);
    }

    public override void Execute()
    {
        Debug.Log("Execute ButtonEl [" + name + "]");
    }
}
