using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    // ワールド情報のリスト
    public static List<WorldInfo> WorldInfos;

    // 現在のワールド情報
    private WorldInfo currentWorldInfo;

    // エンティティ、アイテムの指定ブロックに居るVaxelIdの辞書
    // Keyには3次元ブロック座標が入り、ValueにはVaxelIdのリストが入る
    private Dictionary<int, List<int>> entitiesIDToVaxelIDs;
    private Dictionary<int, List<int>> itemsIDToVaxelIDs;

    // ワールドデータと結びつく各VaxelID。初期値は0
    private int[,,] blocksID;
    private int[,,] entitiesID;
    private int[,,] itemsID;

    // ワールドデータ
    [SerializeField] private WorldData data;

    // プレイヤー
    [SerializeField] private Player player;

    private void Init()
    {
        // ID変換辞書の初期化
        entitiesIDToVaxelIDs = new Dictionary<int, List<int>>();
        itemsIDToVaxelIDs = new Dictionary<int, List<int>>();

        // ワールドの初期化
        blocksID = new int[Constants.WORLD_SIZE, Constants.WORLD_HEIGHT, Constants.WORLD_SIZE];
        entitiesID = new int[Constants.WORLD_SIZE, Constants.WORLD_HEIGHT, Constants.WORLD_SIZE];
        itemsID = new int[Constants.WORLD_SIZE, Constants.WORLD_HEIGHT, Constants.WORLD_SIZE];

        data.Init(ref blocksID, ref entitiesID, ref itemsID);
    }

    public void Create()
    {
        // Paramに保存されているワールド情報を使用してワールドの生成

        // ワールドの初期化
        Init();

        // ワールドの生成
        data.Create(ref blocksID);

        // モブのスポーン
        data.SpawnMob(ref entitiesID);

        // プレイヤーの生成及び配置
        player.Init();
        player.Create();
    }

    public void LoadFromJson()
    {
        // Paramに保存されているワールド情報から指定のJsonファイルを使用して読み込む

        // ワールドの初期化
        Init();
    }

    public void SaveToJson()
    {
        // 現在のワールド情報、データをJsonファイルに保存
    }

    public void DestroyJson()
    {
        // Paramに保存されているワールド情報を使用してワールドを削除
    }

    public static void LoadInfoFromJson()
    {
        WorldInfos = new List<WorldInfo>();

        // JSONファイルから読み込む
    }

    public void Execute()
    {
        // プレイヤーの実行
        player.Execute();
    }
}
