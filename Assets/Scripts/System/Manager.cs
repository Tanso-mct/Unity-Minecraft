using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    // シーンで表示するウィンドウをすべてインスペクターで設定
    [SerializeField] private List<AppWindow> windows = null;

    // ウィンドウの名前をキーにし、リストのインデックスを取得するための変数
    private Dictionary<string, AppWindow> windowNameToIndex = null;

    // BaseAwake関数で最初に必ず実行する。Managerクラスの初期化を行う
    protected void Init(ref Params param)
    {
        windowNameToIndex = new Dictionary<string, AppWindow>();

        for (int i = 0; i < windows.Count; i++)
        {
            windowNameToIndex.Add(windows[i].name, windows[i]);
            windows[i].Init();
        }
    }

    // BaseExit関数で最後に必ず実行する。Managerクラスの終了処理を行う
    protected void Destoroy(ref Params param)
    {

    }

    // 引数にウィンドウの名前を指定し、そのウィンドウを表示する
    protected void ShowWindow(string wndName, ref Params param)
    {
        windows[windows.IndexOf(windowNameToIndex[wndName])].Show();
    }

    // 引数にウィンドウの名前を指定し、そのウィンドウを非表示にする
    protected void CloseWindow(string wndName, ref Params param)
    {
        windows[windows.IndexOf(windowNameToIndex[wndName])].Close();
    }

    // すべての開いているウィンドウのイベントが実行の必要があるなら実行する
    // BaseUpdate関数で実行する
    protected void ExecuteWindows(ref Params param)
    {
        for (int i = 0; i < windows.Count; i++)
        {
            windows[i].Execute(ref param);
        }
    }

    // スクロールすることが検知された場合、ウィンドウをスクロールに合わせて移動させる
    // BaseUpdate関数でExecuteWindows関数のあとに実行する
    protected void ScrollWindows(ref Params param)
    {
        // スクロールによるウィンドウの移動量を取得
        Vector2 moveVec = new Vector2(0, 0);

        // スクロールする必要があるウィンドウの場合、ウィンドウをスクロールに合わせて移動させる
        for (int i = 0; i < windows.Count; i++)
        {
            if (windows[i].IsScroll) windows[i].Move(ref moveVec, ref param);
        }
    }

    // UnityのAwake関数の代わりに使用する関数
    abstract public void BaseAwake(ref Params param);

    // UnityのStart関数の代わりに使用する関数
    abstract public void BaseStart(ref Params param);

    // UnityのUpdate関数の代わりに使用する関数
    abstract public void BaseUpdate(ref Params param);

    // シーンが切り替わる際に呼び出される関数
    abstract public void BaseExit(ref Params param);
}
