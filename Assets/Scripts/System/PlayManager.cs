using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayManager : Manager
{
    [SerializeField] private GameObject wndPlay;

    [SerializeField] private GameObject wndOption;
    [SerializeField] private GameObject wndVideoSetting;
    [SerializeField] private GameObject wndSoundSetting;
    [SerializeField] private GameObject wndControlSetting;
    [SerializeField] private GameObject wndControlSettingScroll;

    [SerializeField] private Player player;

    [SerializeField] private GameObject wndPlayerInventory;

    [SerializeField] private float controlScrollBottom;

    public override void BaseAwake()
    {
        // Managerに設定されているすべてのWindowを初期化
        Init();

        // PlayWindowを表示
        ShowWindow(wndPlay.name);

        //ShowWindow(wndOption.name);
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
        if 
        (
            Input.GetKeyUp(KeyCode.Escape) && 
            wndPlay.gameObject.GetComponent<PlayWindow>().IsOpening &&
            !player.isInventoryOpen
        ){
            ShowOption();
        }

        if (McControls.IsKeyDown(Constants.CONTROL_INVENTORY) && !player.isInventoryOpen)
        {
            ShowPlayerInventory();
        }
        else if (McControls.IsKeyDown(Constants.CONTROL_INVENTORY) && player.isInventoryOpen)
        {
            ClosePlayerInventory();
        }

        // 各ウィンドウの処理を実行
        ExecuteWindows();

        // スクロールされている場合、ウィンドウを移動
        if (wndControlSetting.GetComponent<MenuWindow>().IsOpening) ScrollWindows(controlScrollBottom);
    }

    public override void BaseExit()
    {
        // Managerの終了処理を実行
        Dispose();
    }

    public void ShowOption()
    {
        CloseWindow(wndPlay.name);
        Physics.simulationMode = SimulationMode.Script;

        McControls.CursorLock(false);
        ShowWindow(wndOption.name);
    }

    public void CloseOption()
    {
        CloseWindow(wndOption.name);
        Physics.simulationMode = SimulationMode.FixedUpdate;

        ShowWindow(wndPlay.name);
        McControls.CursorLock(true);
    }

    public void ShowPlayerInventory()
    {
        Debug.Log("ShowPlayerInventory");

        McControls.CursorLock(false);
        player.isInventoryOpen = true;
        ShowWindow(wndPlayerInventory.name);
    }

    public void ClosePlayerInventory()
    {
        Debug.Log("ClosePlayerInventory");

        McControls.CursorLock(true);
        player.isInventoryOpen = false;
        CloseWindow(wndPlayerInventory.name);
    }

    public void ShowVideoSetting()
    {
        Debug.Log("ShowVideoSetting");
        CloseWindow(wndOption.name);
        ShowWindow(wndVideoSetting.name);
    }

    public void CloseVideoSetting()
    {
        Debug.Log("CloseVideoSetting");
        CloseWindow(wndVideoSetting.name);
        ShowWindow(wndOption.name);
    }

    public void ShowSoundSetting()
    {
        Debug.Log("ShowSoundSetting");
        CloseWindow(wndOption.name);
        ShowWindow(wndSoundSetting.name);
    }

    public void CloseSoundSetting()
    {
        Debug.Log("CloseSoundSetting");
        CloseWindow(wndSoundSetting.name);
        ShowWindow(wndOption.name);
    }

    public void ShowControlSetting()
    {
        Debug.Log("ShowControlSetting");
        CloseWindow(wndOption.name);

        ShowWindow(wndControlSetting.name);
        ShowWindow(wndControlSettingScroll.name);
    }

    public void CloseControlSetting()
    {
        Debug.Log("CloseControlSetting");
        CloseWindow(wndControlSetting.name);
        CloseWindow(wndControlSettingScroll.name);

        ShowWindow(wndOption.name);

        Param.popUpWindowDone = true;
    }
}
