using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    // ワールド情報のリスト
    public static List<WorldInfo> WorldInfos;

    // 現在のワールド情報
    private static WorldInfo thisInfo;

    // ブロックのバッファー。その座標に存在しているブロックの種類を示す。
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

    // 描画するブロックの数
    private int[] drawBlockCount;
    private ComputeBuffer drawBlockCountBuff;

    // 描画するブロックのインデックス番号の配列。y, z, wはそれぞれ頂点、UV、頂点インデックスの書きこみはじめ位置。
    private ComputeBuffer drawBlockIndexBuff;

    // メッシュの頂点、UV、頂点インデックスのそれぞれの合計数。
    private int meshVsCount = 0;
    private int meshUVsCount = 0;
    private int meshTrisCount = 0;

    private ComputeBuffer meshVsCountBuff;
    private ComputeBuffer meshUVsCountBuff;
    private ComputeBuffer meshTrisCountBuff;

    // ワールドメッシュの頂点データバッファー。描画範囲によりサイズが変わる。
    private ComputeBuffer meshVsBuff;

    // ワールドメッシュのUVデータバッファー。描画範囲によりサイズが変わる。
    private ComputeBuffer meshUVsBuff;

    // ワールドメッシュの頂点インデックスデータバッファー。描画範囲によりサイズが変わる。
    private ComputeBuffer meshTrisBuff;

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
        blocksIDBuff = new ComputeBuffer(Constants.WORLD_SIZE * Constants.WORLD_HEIGHT * Constants.WORLD_SIZE, sizeof(int));

        blocks = new Dictionary<Vector3Int, Vaxel>();
        entities = new List<Vaxel>();
        items = new List<Vaxel>();

        // Vaxelの管理クラスの初期化
        vaxelAdmin = new VaxelAdmin();
        vaxelAdmin.Init();

        // ワールドメッシュの初期化
        worldMesh = new Mesh();
        objWorldMesh.GetComponent<MeshFilter>().mesh = worldMesh;

        Texture meshAtlasTexture = null;
        SupportFunc.LoadTexture(ref meshAtlasTexture, Constants.TEXTURE_ATLAS_BLOCK);
        objWorldMesh.GetComponent<MeshRenderer>().material.mainTexture = meshAtlasTexture;

        // ソースメッシュオブジェクトの初期化
        meshBlock.Init();

        // ソースメッシュオブジェクトのバッファー作成
        meshBlock.CreateBuffer();

        // ブロックの描画数のバッファー作成
        drawBlockCountBuff = new ComputeBuffer(1, sizeof(int));
        drawBlockCount = new int[1];
        drawBlockCount[0] = 0;
        drawBlockCountBuff.SetData(drawBlockCount);

        // ブロックの情報のバッファー作成
        drawBlockIndexBuff = new ComputeBuffer
        (
            McVideos.RenderDistance * Constants.CHUCK_SIZE * 
            McVideos.RenderDistance * Constants.CHUCK_SIZE * 
            McVideos.RenderDistance * Constants.CHUCK_SIZE, 
            sizeof(int) * 4
        );

        // ワールドメッシュの頂点、UV、頂点インデックスのバッファー作成
        meshVsBuff = new ComputeBuffer
        (
            McVideos.RenderDistance * Constants.CHUCK_SIZE * 
            McVideos.RenderDistance * Constants.CHUCK_SIZE *
            McVideos.RenderDistance * Constants.CHUCK_SIZE * 
            Constants.SOURCE_MESH_VS_MAX, 
            sizeof(float) * 3
        );

        meshUVsBuff = new ComputeBuffer
        (
            McVideos.RenderDistance * Constants.CHUCK_SIZE * 
            McVideos.RenderDistance * Constants.CHUCK_SIZE  *
            McVideos.RenderDistance * Constants.CHUCK_SIZE * 
            Constants.SOURCE_MESH_UVS_MAX, 
            sizeof(float) * 2
        );

        meshTrisBuff = new ComputeBuffer
        (
            McVideos.RenderDistance * Constants.CHUCK_SIZE * 
            McVideos.RenderDistance * Constants.CHUCK_SIZE  *
            McVideos.RenderDistance * Constants.CHUCK_SIZE * 
            Constants.SOURCE_MESH_TRIS_MAX, 
            sizeof(int)
        );

        // メッシュの頂点、UV、頂点インデックスの数のバッファー作成
        int[] countAry = new int[1];
        countAry[0] = 0;
        meshVsCountBuff = new ComputeBuffer(1, sizeof(int));
        meshVsCountBuff.SetData(countAry);

        meshUVsCountBuff = new ComputeBuffer(1, sizeof(int));
        meshUVsCountBuff.SetData(countAry);

        meshTrisCountBuff = new ComputeBuffer(1, sizeof(int));
        meshTrisCountBuff.SetData(countAry);

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

        worldShader.SetInt("PLAYER_X", Constants.WORLD_HALF_SIZE + 0);
        worldShader.SetInt("PLAYER_Y", 4);
        worldShader.SetInt("PLAYER_Z", Constants.WORLD_HALF_SIZE + 0);

        worldShader.SetInt("RENDER_DISTANCE", McVideos.RenderDistance);

        worldShader.SetBuffer(0, "drawBlockCount", drawBlockCountBuff);
        worldShader.SetBuffer(0, "drawBlockIndex", drawBlockIndexBuff);

        worldShader.SetBuffer(0, "meshVs", meshVsBuff);
        worldShader.SetBuffer(0, "meshUVs", meshUVsBuff);
        worldShader.SetBuffer(0, "meshTris", meshTrisBuff);

        worldShader.SetBuffer(0, "meshVsCount", meshVsCountBuff);
        worldShader.SetBuffer(0, "meshUVsCount", meshUVsCountBuff);
        worldShader.SetBuffer(0, "meshTrisCount", meshTrisCountBuff);

        meshBlock.SetBuffer(ref worldShader, "sourceMeshBlockVs", "sourceMeshBlockUVs", "sourceMeshBlockTris");

        if (thisInfo.worldType == "Flat")
        {
            // フラットワールドの生成
            int generateFlatWorld = worldShader.FindKernel("GenerateFlatWorld");
            worldShader.Dispatch(generateFlatWorld, threadGroupsX, threadGroupsY, threadGroupsZ);
            
            int getBlocksAdjacentAir = worldShader.FindKernel("GetBlocksAdjacentAir");
            worldShader.Dispatch(getBlocksAdjacentAir, threadGroupsX, threadGroupsY, threadGroupsZ);
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
