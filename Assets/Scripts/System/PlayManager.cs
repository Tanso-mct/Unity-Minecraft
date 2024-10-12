using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayManager : Manager
{
    [SerializeField] private GameObject wndPlay;

    public override void BaseAwake()
    {
        // Managerに設定されているすべてのWindowを初期化
        Init();

        // PlayWindowを表示
        ShowWindow(wndPlay.name);
    }

    public override void BaseStart()
    {
        // 各ウィンドウの処理を実行
        ExecuteWindows();

        // スクロールされている場合、ウィンドウを移動
        ScrollWindows();
    }

    public override void BaseUpdate()
    {
        // 各ウィンドウの処理を実行
        ExecuteWindows();

        // 各ウィンドウの処理を実行
        ScrollWindows();
    }

    public override void BaseExit()
    {
        // Managerの終了処理を実行
        Dispose();
    }
}
