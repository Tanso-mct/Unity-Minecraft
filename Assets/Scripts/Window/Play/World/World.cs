using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    // ワールド情報のリスト
    public static List<WorldInfo> WorldInfos;

    // 現在のワールド情報
    private WorldInfo currentWorldInfo;

    // ワールドデータと結びつく各VaxelID。初期値は0。その座標ブロックに存在しているかを示す。
    private int[,,] blocksID;
    private int[,,] entitiesID;
    private int[,,] itemsID;

    // ワールドデータ
    [SerializeField] private GameObject dataObj;
    private WorldData data;

    // プレイヤー
    [SerializeField] private Player player;

    // 各種形態のマネージャー
    private BlockManager blockMgr;
    private ItemManager itemMgr;
    private EntityManager entityMgr;

    private int debugNum = 0;


    private void Init()
    {
        // データの取得
        data = dataObj.GetComponent<WorldData>();

        // ワールドの初期化
        blocksID = new int[Constants.WORLD_SIZE, Constants.WORLD_HEIGHT, Constants.WORLD_SIZE];
        entitiesID = new int[Constants.WORLD_SIZE, Constants.WORLD_HEIGHT, Constants.WORLD_SIZE];
        itemsID = new int[Constants.WORLD_SIZE, Constants.WORLD_HEIGHT, Constants.WORLD_SIZE];

        // マネージャーの初期化
        blockMgr = new BlockManager();
        blockMgr.Init();

        itemMgr = new ItemManager();
        itemMgr.Init();

        entityMgr = new EntityManager();
        entityMgr.Init();

        data.Init();
    }

    public void Create()
    {
        // Paramに保存されているワールド情報を使用してワールドの生成

        // ワールドの初期化
        Init();

        // ワールドの生成
        data.Create(dataObj, ref blocksID, ref entitiesID, ref itemsID, ref blockMgr, ref itemMgr, ref entityMgr);

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

        if (Input.GetKeyDown(KeyCode.F1))
        {
            // デバッグ用
            for (int x = -McVideos.RenderDistance * 8; x < McVideos.RenderDistance * 8; x++)
            {
                for (int z = -McVideos.RenderDistance * 8; z < McVideos.RenderDistance * 8; z++)
                {
                    data.CreateVaxel(Constants.VAXEL_TYPE.BEDROCK, new Vector3(x, debugNum, z), ref blocksID, ref blockMgr, dataObj);
                }
            }

            debugNum++;
        }
    }
}
