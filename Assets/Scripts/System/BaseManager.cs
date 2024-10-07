    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour
{
    // Managerクラスを継承したクラスを持つゲームオブジェクトを設定
    [SerializeField] private GameObject managerObject;
    private Manager manager;

    /* 
     * 今回のプロトタイプ版では以下の関数でのみUnityのAwake、Start、Update関数を使用する
     * 他のスクリプトでこれらを使うことは推奨はしないが、作業時間上許容する場合もある
     */
    private void Awake()
    {
        // FPSを設定
        SetFps();

        // メッセージ処理のためのパラメータを初期化
        param.Init();

        // シリアル通信スクリプトを持つゲームオブジェクトを生成
        sensorDevice.Create();

        // Managerクラスを継承したクラスを持つゲームオブジェクトからManagerクラスを取得
        manager = managerObject.GetComponent<Manager>();

        // Managerクラスを継承したクラスのAwake関数を実行
        manager.BaseAwake(ref param);
    }

    void Start()
    {
        // Managerクラスを継承したクラスのStart関数を実行
        manager.BaseStart(ref param);
    }

    void Update()
    {
        // Managerクラスを継承したクラスのUpdate関数を実行
        manager.BaseUpdate(ref param);
    }

    // シーンが切り替わる際に呼び出される関数
    private void Exit()
    {
        // Managerクラスを継承したクラスのExit関数を実行
        manager.BaseExit(ref param);
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
