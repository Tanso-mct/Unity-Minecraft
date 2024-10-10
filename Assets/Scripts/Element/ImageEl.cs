using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageEl : Element
{
    public override void Init()
    {
        Debug.Log("Init ImageEl [" + name + "]");

        // 画像の初期化処理
        InitImages();
    }

    public override void Show()
    {
        Debug.Log("Show ImageEl [" + name + "]");

        // 画像の表示処理
        ShowImages(true);
    }

    public override void Close()
    {
        Debug.Log("Close ImageEl [" + name + "]");

        // 画像の非表示処理
        ShowImages(false);
    }

    public override void Execute()
    {
        
    }
}
