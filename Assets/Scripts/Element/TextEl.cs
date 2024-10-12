using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TextEl : Element
{
    public UnityEvent hoverEvent = null;

    public override void Init()
    {
        // 継承元クラスの初期化処理を実行
        BaseInit();
    }

    public override void Show()
    {
        // 画像の表示処理
        ShowImages(true, initGroup.name);
    }

    public override void Close()
    {
        // 画像の非表示処理
        ShowImages(false, initGroup.name);
    }

    public override void Execute()
    {
        if (IsHover() && hoverEvent != null) hoverEvent.Invoke();
    }
}
