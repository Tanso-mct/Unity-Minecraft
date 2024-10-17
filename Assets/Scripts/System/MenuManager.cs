using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : Manager
{
    [SerializeField] private GameObject wndLoad;
    [SerializeField] private GameObject wndTitle;
    [SerializeField] private GameObject wndOption;
    [SerializeField] private GameObject wndVideoSetting;
    [SerializeField] private GameObject wndSoundSetting;
    [SerializeField] private GameObject wndControlSetting;

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
            ShowWindow(wndTitle.name);
        }
    }

    public override void BaseExit()
    {
        // Managerの終了処理を実行
        Dispose();
    }

    public void ShowOption()
    {
        CloseWindow(wndTitle.name);
        ShowWindow(wndOption.name);
    }

    public void CloseOption()
    {
        CloseWindow(wndOption.name);
        ShowWindow(wndTitle.name);
    }

    public void ShowVideoSetting()
    {
        CloseWindow(wndOption.name);
        ShowWindow(wndVideoSetting.name);
    }

    public void CloseVideoSetting()
    {
        CloseWindow(wndVideoSetting.name);
        ShowWindow(wndOption.name);
    }

    public void ShowSoundSetting()
    {
        CloseWindow(wndOption.name);
        ShowWindow(wndSoundSetting.name);
    }

    public void CloseSoundSetting()
    {
        CloseWindow(wndSoundSetting.name);
        ShowWindow(wndOption.name);
    }

    public void ShowControlSetting()
    {
        CloseWindow(wndTitle.name);
        CloseWindow(wndOption.name);
        ShowWindow(wndControlSetting.name);
    }

    public void CloseControlSetting()
    {
        CloseWindow(wndControlSetting.name);
        ShowWindow(wndOption.name);
    }
}
