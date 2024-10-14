using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : Manager
{
    [SerializeField] private GameObject wndLoad;
    [SerializeField] private GameObject wndMenu;
    [SerializeField] private GameObject wndOption;

    [SerializeField] private int loadingShowFrame = 120;
    private int loadingFrame = 0;

    public override void BaseAwake()
    {
        // Managerに設定されているすべてのWindowを初期化
        Init();

        // MenuWindowを表示
        ShowWindow(wndLoad.name);
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

        if (loadingFrame < loadingShowFrame)
        {
            loadingFrame++;
        }
        else
        {
            CloseWindow(wndLoad.name);
            ShowWindow(wndMenu.name);
        }
    }

    public void ShowOption()
    {
        CloseWindow(wndMenu.name);
        ShowWindow(wndOption.name);
    }

    public void CloseOption()
    {
        CloseWindow(wndOption.name);
        ShowWindow(wndMenu.name);
    }

    public override void BaseExit()
    {
        // Managerの終了処理を実行
        Dispose();
    }
}
