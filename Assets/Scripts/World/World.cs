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

    // 各Vaxelのメッシュ状態における頂点群と各バッファー
    private Vector3[] baseBlockVs;
    private Vector3[] baseStairVs;
    private Vector3[] baseSlabVs;

    private ComputeBuffer baseBlockVsBuff;
    private ComputeBuffer baseStairVsBuff;
    private ComputeBuffer baseSlabVsBuff;

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

        // ベースとなるブロックの頂点群を生成
        baseBlockVs = new Vector3[Constants.BLOCK_VERTEX_COUNT];
        baseBlockVs[0] = new Vector3(-0.5f, 0.5f, -0.5f);
        baseBlockVs[1] = new Vector3(0.5f, 0.5f, -0.5f);
        baseBlockVs[2] = new Vector3(0.5f, -0.5f, -0.5f);
        baseBlockVs[3] = new Vector3(-0.5f, -0.5f, -0.5f);

        baseBlockVs[4] = new Vector3(-0.5f, 0.5f, 0.5f);
        baseBlockVs[5] = new Vector3(0.5f, 0.5f, 0.5f);
        baseBlockVs[6] = new Vector3(0.5f, -0.5f, 0.5f);
        baseBlockVs[7] = new Vector3(-0.5f, -0.5f, 0.5f);

        baseBlockVsBuff = new ComputeBuffer(Constants.BLOCK_VERTEX_COUNT, sizeof(float) * 3, ComputeBufferType.Structured);
        baseBlockVsBuff.SetData(baseBlockVs);

        // ベースとなる階段の頂点群を生成
        baseStairVs = new Vector3[Constants.STAIR_VERTEX_COUNT];
        baseStairVs[0] = new Vector3(-0.5f, 0.5f, 0f);
        baseStairVs[1] = new Vector3(0.5f, 0.5f, 0f);
        baseStairVs[2] = new Vector3(0.5f, 0, 0f);
        baseStairVs[3] = new Vector3(-0.5f, 0, 0f);

        baseStairVs[4] = new Vector3(-0.5f, 0f, -0.5f);
        baseStairVs[5] = new Vector3(0.5f, 0f, -0.5f);
        baseStairVs[6] = new Vector3(0.5f, -0.5f, -0.5f);
        baseStairVs[7] = new Vector3(-0.5f, -0.5f, -0.5f);

        baseStairVs[8] = new Vector3(-0.5f, 0.5f, 0.5f);
        baseStairVs[9] = new Vector3(0.5f, 0.5f, 0.5f);
        baseStairVs[10] = new Vector3(0.5f, -0.5f, 0.5f);
        baseStairVs[11] = new Vector3(-0.5f, -0.5f, 0.5f);

        baseStairVsBuff = new ComputeBuffer(Constants.STAIR_VERTEX_COUNT, sizeof(float) * 3);
        baseStairVsBuff.SetData(baseStairVs);

        // ベースとなる半ブロックの頂点群を生成
        baseSlabVs = new Vector3[Constants.SLAB_VERTEX_COUNT];
        baseSlabVs[0] = new Vector3(-0.5f, 0, -0.5f);
        baseSlabVs[1] = new Vector3(0.5f, 0, -0.5f);
        baseSlabVs[2] = new Vector3(0.5f, -0.5f, -0.5f);
        baseSlabVs[3] = new Vector3(-0.5f, -0.5f, -0.5f);

        baseSlabVs[4] = new Vector3(-0.5f, 0, 0.5f);
        baseSlabVs[5] = new Vector3(0.5f, 0, 0.5f);
        baseSlabVs[6] = new Vector3(0.5f, -0.5f, 0.5f);
        baseSlabVs[7] = new Vector3(-0.5f, -0.5f, 0.5f);

        baseSlabVsBuff = new ComputeBuffer(Constants.SLAB_VERTEX_COUNT, sizeof(float) * 3);
        baseSlabVsBuff.SetData(baseSlabVs);

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

        // 各バッファーをシェーダーにセット
        worldShader.SetBuffer(0, "blocksID", blocksIDBuff);

        worldShader.SetBuffer(0, "baseBlockVs", baseBlockVsBuff);
        worldShader.SetBuffer(0, "baseStairVs", baseStairVsBuff);
        worldShader.SetBuffer(0, "baseSlabVs", baseSlabVsBuff);

        // シェーダーの定数をセット
        Constants.SetShaderConstants(ref worldShader);
        
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
        if (baseBlockVsBuff != null) baseBlockVsBuff.Release();
        if (baseStairVsBuff != null) baseStairVsBuff.Release();
        if (baseSlabVsBuff != null) baseSlabVsBuff.Release();
    }
}
