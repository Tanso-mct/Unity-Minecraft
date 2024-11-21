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
    [SerializeField] private GameObject wndControlSettingScroll;
    [SerializeField] private GameObject wndSinglePlayer;
    [SerializeField] private GameObject wndSinglePlayerScroll;

    [SerializeField] private int loadingShowFrame = 120;

    [SerializeField] private float controlScrollBottom;
    [SerializeField] private float singlePlayerScrollBottom;

    private int loadingFrame = 0;

    [SerializeField] private McSounds mcSounds;

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
        if (wndControlSetting.GetComponent<MenuWindow>().IsOpening)
        {
            ScrollWindows(controlScrollBottom);
        }
        else if (wndSinglePlayer.GetComponent<MenuWindow>().IsOpening)
        {
            ScrollWindows(singlePlayerScrollBottom);
        }
    }

    public override void BaseUpdate()
    {
        // 各ウィンドウの処理を実行
        ExecuteWindows();

        // スクロールされている場合、ウィンドウを移動
        if (wndControlSetting.GetComponent<MenuWindow>().IsOpening)
        {
            ScrollWindows(controlScrollBottom);
        }
        else if (wndSinglePlayer.GetComponent<MenuWindow>().IsOpening)
        {
            ScrollWindows(singlePlayerScrollBottom);
        }

        if (loadingFrame < loadingShowFrame)
        {
            loadingFrame++;
        }
        else if (loadingFrame == loadingShowFrame)
        {
            loadingFrame++; 
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
        Debug.Log("ShowOption");
        CloseWindow(wndTitle.name);
        ShowWindow(wndOption.name);

        mcSounds.PlayUI(Constants.SOUND_CLICK);
    }

    public void CloseOption()
    {
        Debug.Log("CloseOption");
        CloseWindow(wndOption.name);
        ShowWindow(wndTitle.name);

        mcSounds.PlayUI(Constants.SOUND_CLICK);
    }

    public void ShowVideoSetting()
    {
        Debug.Log("ShowVideoSetting");
        CloseWindow(wndOption.name);
        ShowWindow(wndVideoSetting.name);

        mcSounds.PlayUI(Constants.SOUND_CLICK);
    }

    public void CloseVideoSetting()
    {
        Debug.Log("CloseVideoSetting");
        CloseWindow(wndVideoSetting.name);
        ShowWindow(wndOption.name);

        mcSounds.PlayUI(Constants.SOUND_CLICK);
    }

    public void ShowSoundSetting()
    {
        Debug.Log("ShowSoundSetting");
        CloseWindow(wndOption.name);
        ShowWindow(wndSoundSetting.name);

        mcSounds.PlayUI(Constants.SOUND_CLICK);
    }

    public void CloseSoundSetting()
    {
        Debug.Log("CloseSoundSetting");
        CloseWindow(wndSoundSetting.name);
        ShowWindow(wndOption.name);

        mcSounds.PlayUI(Constants.SOUND_CLICK);
    }

    public void ShowControlSetting()
    {
        Debug.Log("ShowControlSetting");
        CloseWindow(wndOption.name);
        
        ShowWindow(wndControlSetting.name);
        ShowWindow(wndControlSettingScroll.name);

        mcSounds.PlayUI(Constants.SOUND_CLICK);
    }

    public void CloseControlSetting()
    {
        Debug.Log("CloseControlSetting");
        CloseWindow(wndControlSetting.name);
        CloseWindow(wndControlSettingScroll.name);

        ShowWindow(wndOption.name);

        Param.popUpWindowDone = true;

        mcSounds.PlayUI(Constants.SOUND_CLICK);
    }

    public void ShowSinglePlayer()
    {
        Debug.Log("ShowSinglePlayer");
        CloseWindow(wndTitle.name);

        ShowWindow(wndSinglePlayer.name);
        ShowWindow(wndSinglePlayerScroll.name);

        mcSounds.PlayUI(Constants.SOUND_CLICK);
    }

    public void CloseSinglePlayer()
    {
        Debug.Log("CloseSinglePlayer");
        CloseWindow(wndSinglePlayer.name);
        CloseWindow(wndSinglePlayerScroll.name);

        ShowWindow(wndTitle.name);

        Param.popUpWindowDone = true;

        mcSounds.PlayUI(Constants.SOUND_CLICK);
    }

    public void QuitGame()
    {
        Param.msg = Constants.MSG_QUIT_GAME;

        mcSounds.PlayUI(Constants.SOUND_CLICK);
    }

    public void PlaySelectWorld()
    {
        Debug.Log("PlaySelectWorld");

        mcSounds.PlayUI(Constants.SOUND_CLICK);
    }

    public void CreateNewWorld()
    {
        Debug.Log("CreateNewWorld");

        mcSounds.PlayUI(Constants.SOUND_CLICK);
    }

    public void DestroySelectWorld()
    {
        Debug.Log("DestroySelectWorld");

        mcSounds.PlayUI(Constants.SOUND_CLICK);
    }
}
