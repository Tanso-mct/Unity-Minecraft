using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayWindow : GameWindow
{
    [SerializeField] World world;   

    public override void Init()
    {
        // 各キャンバスとパネルを非表示にする
        ShowCanvases(false);
        ShowBackgrounds(false);

        // 各辞書変数を初期化
        diImageEl = new Dictionary<string, Element>();
        diTextEl = new Dictionary<string, Element>();

        // 各要素を取得
        GetElements(ref images, ref diImageEl);
        GetElements(ref texts, ref diTextEl);

        // 各要素を初期化
        ElementsInit(ref diImageEl);
        ElementsInit(ref diTextEl);

        // Debug時のみWorldInfoを生成
        World.CreateWorldInfo("Test", Constants.WORLD_MODE_CREATIVE, Constants.WORLD_TYPE_FLAT);
        World.SetWorldInfo(0);

        // Paramの値からワールドを生成
        world.Init();
    }

    public override void Show()
    {
        // 開いている場合は処理しない
        if (isOpening) return;
        isOpening = true;

        // キャンバスとパネルを表示
        ShowCanvases(true);
        ShowBackgrounds(true);

        // 各要素を表示
        ElementsShow(ref diImageEl);
        ElementsShow(ref diTextEl);
    }

    public override void Close()
    {
        // 開いていない場合は処理しない
        if (!isOpening) return;
        isOpening = false;

        // キャンバスとパネルを非表示
        ShowCanvases(false);
        ShowBackgrounds(false);

        // 各要素を非表示
        ElementsClose(ref diImageEl);
        ElementsClose(ref diTextEl);
    }

    public override void Execute()
    {
        // 開いていない場合は処理しない
        if (!isOpening) return;

        world.Execute();
    }

    public override void Move(ref Vector2 moveVec)
    {
        if (!isOpening) return;

        // 各要素を移動
        ElementsMove(ref diImageEl, ref moveVec);
        ElementsMove(ref diTextEl, ref moveVec);
    }
}
