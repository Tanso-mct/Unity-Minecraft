using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    // ワールド情報のリスト
    public static List<WorldInfo> WorldInfos;

    // 現在のワールド情報
    private static WorldInfo thisInfo;

    // 初期値は0とし空気を示す。その座標に存在しているブロックの種類を示す。
    private int[,,] blocksID;

    // ブロックのバッファー
    private ComputeBuffer blocksIDBuff;

    // ワールド上のブロックとエンティティとアイテムのオブジェクト
    Dictionary<Vector3Int, Vaxel> blocks;
    List<Vaxel> entities;
    List<Vaxel> items;

    // ワールドメッシュオブジェクト
    [SerializeField] private GameObject objWorldMesh;
    private Mesh worldMesh;

    // 各Vaxelのソースメッシュオブジェクト
    [SerializeField] private WorldMesh meshBlock;

    // プレイヤー
    [SerializeField] private Player player;

    // Vaxelの管理クラス
    private VaxelAdmin vaxelAdmin;

    // Shader
    [SerializeField] private ComputeShader worldShader;

    public static void LoadInfoFromJson()
    {
        WorldInfos = new List<WorldInfo>();

        // 読み込み
    }

    public static void CreateWorldInfo(string name, string gameMode, string worldType)
    {
        WorldInfo worldInfo = new WorldInfo();
        worldInfo.worldName = name;
        worldInfo.gameMode = gameMode;
        worldInfo.worldType = worldType;

        WorldInfos.Add(worldInfo);
    }

    public static void SetWorldInfo(int infoIndex)
    {
        thisInfo = WorldInfos[infoIndex];
    }

    private void SaveWorldInfo()
    {
        // JSONファイルに保存
    }

    public void Init()
    {
        // ワールドの初期化
        blocksID = new int[Constants.WORLD_SIZE, Constants.WORLD_HEIGHT, Constants.WORLD_SIZE];
        blocksIDBuff = new ComputeBuffer(Constants.WORLD_SIZE * Constants.WORLD_HEIGHT * Constants.WORLD_SIZE, sizeof(int));
        blocksIDBuff.SetData(blocksID);

        blocks = new Dictionary<Vector3Int, Vaxel>();
        entities = new List<Vaxel>();
        items = new List<Vaxel>();

        // Vaxelの管理クラスの初期化
        vaxelAdmin = new VaxelAdmin();
        vaxelAdmin.Init();

        // ワールドメッシュの初期化
        worldMesh = new Mesh();
        objWorldMesh.GetComponent<MeshFilter>().mesh = worldMesh;

        // Texture meshAtlasTexture = null;
        // SupportFunc.LoadTexture(ref meshAtlasTexture, Constants.TEXTURE_ATLAS_BLOCK);
        // objWorldMesh.GetComponent<MeshRenderer>().material.mainTexture = meshAtlasTexture;

        // ソースメッシュオブジェクトの初期化
        meshBlock.Init();

        // ソースメッシュオブジェクトのバッファー作成
        meshBlock.CreateBuffer();

        // ワールドの生成もしくは読み込み
        if(thisInfo.dataJsonPath == "") Create(thisInfo.worldType);
        else LoadFromJson();
    }

    // Paramに保存されているワールド情報を使用してワールドの生成
    public void Create(string worldType)
    {
        // メッシュの生成
        int threadGroupsX = Mathf.CeilToInt(Constants.WORLD_SIZE / 8.0f);
        int threadGroupsY = Mathf.CeilToInt(Constants.WORLD_HEIGHT / 8.0f);
        int threadGroupsZ = Mathf.CeilToInt(Constants.WORLD_SIZE / 8.0f);

        // シェーダーの定数をセット
        Constants.SetShaderConstants(ref worldShader);

        // 各バッファーをシェーダーにセット
        worldShader.SetBuffer(0, "blocksID", blocksIDBuff);
        meshBlock.SetBuffer(ref worldShader, "sourceMeshBlockVs", "sourceMeshBlockUVs", "sourceMeshBlockTris");

        if (thisInfo.worldType == "Flat")
        {
            // フラットワールドの生成
            int generateFlatWorld = worldShader.FindKernel("GenerateFlatWorld");
            worldShader.Dispatch(generateFlatWorld, threadGroupsX, threadGroupsY, threadGroupsZ);
        }
        else 
        {
            // ダイアモンドスクエアアルゴリズムによるワールドの生成
            int diamondSquareStep = worldShader.FindKernel("DiamondSquareStep");
            worldShader.Dispatch(diamondSquareStep, threadGroupsX, threadGroupsY, threadGroupsZ);
        }

        // プレイヤーの生成及び配置
        player.Init();
        player.Create();
    }

    // Paramに保存されているワールド情報から指定のJsonファイルを使用して読み込む
    public void LoadFromJson()
    {
        // メッシュの読み込み

        // プレイヤーの読み込み及び配置
        player.Init();
        player.LoadFromJson();
    }

    public void SaveToJson()
    {
        // 現在のワールド情報、データをJsonファイルに保存
    }

    public void DestroyJson()
    {
        // Paramに保存されているワールド情報を使用してワールドを削除
    }

    private void MeshUpdate()
    {

    }

    private void BlockUpdate()
    {

    }

    private void EntityUpdate()
    {

    }

    private void ItemUpdate()
    {

    }

    public void Execute()
    {
        // プレイヤーの実行
        player.Execute();

        // メッシュの更新
        MeshUpdate();

        // ブロックの更新
        BlockUpdate();

        // エンティティの更新
        EntityUpdate();

        // アイテムの更新
        ItemUpdate();
    }

    void OnDestroy()
    {
        // バッファを解放
        if (blocksIDBuff != null) blocksIDBuff.Release();
        
        meshBlock.ReleaseBuffer();
    }
}
