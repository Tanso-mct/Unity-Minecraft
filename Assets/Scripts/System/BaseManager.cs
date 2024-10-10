    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour
{
    // Managerクラスを継承したクラスを持つゲームオブジェクトを設定
    [SerializeField] private GameObject managerObject;
    private Manager manager;

    private void Awake()
    {
        // メッセージ処理のためのパラメータを初期化
        Param.Init();

        // Managerクラスを継承したクラスを持つゲームオブジェクトからManagerクラスを取得
        manager = managerObject.GetComponent<Manager>();

        // Managerクラスを継承したクラスのAwake関数を実行
        manager.BaseAwake();
    }

    void Start()
    {
        // メッセージ処理のためのパラメータを初期化
        Param.Init();

        // FPSを設定
        SetFps();

        // Managerクラスを継承したクラスのStart関数を実行
        manager.BaseStart();
    }

    void Update()
    {
        // メッセージ処理のためのパラメータを初期化
        Param.Init();

        // Managerクラスを継承したクラスのUpdate関数を実行
        manager.BaseUpdate();
    }

    // シーンが切り替わる際に呼び出される関数
    private void Exit()
    {
        // Managerクラスを継承したクラスのExit関数を実行
        manager.BaseExit();
    }

    // 各シーンで必ず一度実行する。FPSの設定を行う。
    public int SetFps()
    {
        return Constants.MSG_NULL;
    }

    // paramに格納されたシーン名を元に、シーンを変更する。
    public int ChangeScene()
    {
        return Constants.MSG_NULL;
    }
}
