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

    // [0] 描画するブロックの数 [1][2][3] ブロックのインデックス、頂点の開始位置、頂点インデックスの開始位置
    private ComputeBuffer countsBuff;
    private int drawBlockCount = 0;
    private int meshVsCount = 0;
    private int meshTrisCount = 0;


    // ワールドメッシュの頂点データバッファー。描画範囲によりサイズが変わる。
    private ComputeBuffer meshVsBuff;

    // ワールドメッシュのUVデータバッファー。描画範囲によりサイズが変わる。
    private ComputeBuffer meshUVsBuff;

    // ワールドメッシュの頂点インデックスデータバッファー。描画範囲によりサイズが変わる。
    private ComputeBuffer meshTrisBuff;

    // ソースブロックらのメッシュデータ
    List<Vector3> sourceMeshVs;
    List<Vector2> sourceMeshUVs;
    List<int> sourceMeshTris;
    private ComputeBuffer sourceMeshVsBuff;
    private ComputeBuffer sourceMeshUVsBuff;
    private ComputeBuffer sourceMeshTrisBuff;

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

        // ソースメッシュオブジェクトの初期化
        sourceMeshVs = new List<Vector3>();
        sourceMeshUVs = new List<Vector2>();
        sourceMeshTris = new List<int>();
        
        meshBlock.Init();

        // ソースメッシュオブジェクトのデータを作成
        meshBlock.SetData(ref worldShader, ref sourceMeshVs, ref sourceMeshUVs, ref sourceMeshTris);

        // ソースメッシュオブジェクトのバッファー作成
        sourceMeshVsBuff = new ComputeBuffer(sourceMeshVs.Count, sizeof(float) * 3);
        sourceMeshUVsBuff = new ComputeBuffer(sourceMeshUVs.Count, sizeof(float) * 2);
        sourceMeshTrisBuff = new ComputeBuffer(sourceMeshTris.Count, sizeof(int));

        sourceMeshVsBuff.SetData(sourceMeshVs);
        sourceMeshUVsBuff.SetData(sourceMeshUVs);
        sourceMeshTrisBuff.SetData(sourceMeshTris);

        // ワールドメッシュの初期化
        worldMesh = new Mesh();
        objWorldMesh.GetComponent<MeshFilter>().mesh = worldMesh;

        Texture meshAtlasTexture = null;
        SupportFunc.LoadTexture(ref meshAtlasTexture, Constants.TEXTURE_ATLAS_BLOCK);
        objWorldMesh.GetComponent<MeshRenderer>().material.mainTexture = meshAtlasTexture;

        // [0] 描画するブロックの数 [1][2] 頂点の開始位置、頂点インデックスの開始位置を示すバッファー
        countsBuff = new ComputeBuffer(3, sizeof(int));

        int[] countsAry = new int[3];
        for (int i = 0; i < 3; i++) countsAry[i] = 0;
        countsBuff.SetData(countsAry);
        
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
            Constants.SOURCE_MESH_VS_MAX, 
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

        // ワールドの生成もしくは読み込み
        if(thisInfo.dataJsonPath == "") Create(thisInfo.worldType);
        else LoadFromJson();
    }

    public void SetGenerateBuffer(int kernelIndex)
    {
        // シェーダーの定数をセット
        Constants.SetShaderConstants(ref worldShader);

        // 各バッファーをシェーダーにセット
        worldShader.SetBuffer(kernelIndex, "blocksID", blocksIDBuff);

        worldShader.SetInt("RENDER_DISTANCE", McVideos.RenderDistance);

        worldShader.SetBuffer(kernelIndex, "counts", countsBuff);

        worldShader.SetBuffer(kernelIndex, "meshVs", meshVsBuff);
        worldShader.SetBuffer(kernelIndex, "meshUVs", meshUVsBuff);
        worldShader.SetBuffer(kernelIndex, "meshTris", meshTrisBuff);

        worldShader.SetBuffer(kernelIndex, "sourceMeshVs", sourceMeshVsBuff);
        worldShader.SetBuffer(kernelIndex, "sourceMeshUVs", sourceMeshUVsBuff);
        worldShader.SetBuffer(kernelIndex, "sourceMeshTris", sourceMeshTrisBuff);
    }

    public void SetMeshUpdateBuffer(int kernelIndex)
    {
        // シェーダーの定数をセット
        Constants.SetShaderConstants(ref worldShader);

        // 各バッファーをシェーダーにセット
        worldShader.SetBuffer(kernelIndex, "blocksID", blocksIDBuff);

        worldShader.SetInt("RENDER_DISTANCE", McVideos.RenderDistance);
    }

    // Paramに保存されているワールド情報を使用してワールドの生成
    public void Create(string worldType)
    {
        // プレイヤーの初期化
        player.Init();

        // ワールド全体を処理するためのスレッドグループ数
        int worldThreadGroupsX = Mathf.CeilToInt(Constants.WORLD_SIZE / 8.0f);
        int worldThreadGroupsY = Mathf.CeilToInt(Constants.WORLD_HEIGHT / 8.0f);
        int worldThreadGroupsZ = Mathf.CeilToInt(Constants.WORLD_SIZE / 8.0f);

        // プレイヤーの座標を変換
        Vector3Int convertedPos = SupportFunc.CoordsIntConvert(player.Pos);

        // 描画範囲を処理するためのスレッドグループ数
        Vector3Int worldOrigin = new Vector3Int
        (
            convertedPos.x - McVideos.RenderDistance * Constants.CHUCK_SIZE,
            convertedPos.y - McVideos.RenderDistance * Constants.CHUCK_SIZE,
            convertedPos.z - McVideos.RenderDistance * Constants.CHUCK_SIZE
        );

        Vector3Int worldOpposite = new Vector3Int
        (
            convertedPos.x + McVideos.RenderDistance * Constants.CHUCK_SIZE,
            convertedPos.y + McVideos.RenderDistance * Constants.CHUCK_SIZE,
            convertedPos.z + McVideos.RenderDistance * Constants.CHUCK_SIZE
        );

        worldOrigin.x = Mathf.Clamp(worldOrigin.x, 0, Constants.WORLD_SIZE - 1);
        worldOrigin.y = Mathf.Clamp(worldOrigin.y, 0, Constants.WORLD_HEIGHT - 1);
        worldOrigin.z = Mathf.Clamp(worldOrigin.z, 0, Constants.WORLD_SIZE - 1);

        worldOpposite.x = Mathf.Clamp(worldOpposite.x, 0, Constants.WORLD_SIZE - 1);
        worldOpposite.y = Mathf.Clamp(worldOpposite.y, 0, Constants.WORLD_HEIGHT - 1);
        worldOpposite.z = Mathf.Clamp(worldOpposite.z, 0, Constants.WORLD_SIZE - 1);

        int viewThreadGroupsX = Mathf.CeilToInt((worldOpposite.x - worldOrigin.x) / 8.0f);
        int viewThreadGroupsY = Mathf.CeilToInt((worldOpposite.y - worldOrigin.y) / 8.0f);
        int viewThreadGroupsZ = Mathf.CeilToInt((worldOpposite.z - worldOrigin.z) / 8.0f);

        // 処理するインデックスの開始地点、終了地点をセット
        worldShader.SetInt("VIEW_ORIGIN_X", worldOrigin.x);
        worldShader.SetInt("VIEW_ORIGIN_Y", worldOrigin.y);
        worldShader.SetInt("VIEW_ORIGIN_Z", worldOrigin.z);

        if (thisInfo.worldType == "Flat")
        {
            // フラットワールドの生成
            int generateFlatWorld = worldShader.FindKernel("GenerateFlatWorld");
            SetGenerateBuffer(generateFlatWorld);
            worldShader.Dispatch(generateFlatWorld, worldThreadGroupsX, worldThreadGroupsY, worldThreadGroupsZ);
        }
        else 
        {
            // ダイアモンドスクエアアルゴリズムによるワールドの生成
            int diamondSquareStep = worldShader.FindKernel("DiamondSquareStep");
            worldShader.Dispatch(diamondSquareStep, worldThreadGroupsX, worldThreadGroupsY, worldThreadGroupsZ);
        }

        // 空気と隣接するブロックを描画ブロックとし、それらの情報を取得
        int meshGenerate = worldShader.FindKernel("MeshGenerate");
        SetGenerateBuffer(meshGenerate);
        worldShader.Dispatch(meshGenerate, viewThreadGroupsX, viewThreadGroupsY, viewThreadGroupsZ);

        // 各数を取得
        int[] countsAry = new int[3];
        countsBuff.GetData(countsAry);

        drawBlockCount = countsAry[0];
        meshVsCount = countsAry[1];
        meshTrisCount = countsAry[2];

        Debug.Log("Draw Block Count : " + drawBlockCount);

        Debug.Log("Mesh Vs Count : " + meshVsCount);
        Debug.Log("Mesh Tris Count : " + meshTrisCount);

        Debug.Log("SOURCE_MESH_VS_MAX : " + Constants.SOURCE_MESH_VS_MAX);
        Debug.Log("SOURCE_MESH_TRIS_MAX : " + Constants.SOURCE_MESH_TRIS_MAX);

        // Debug.Log("\nDraw Block Count : " + drawBlockCount[0]);
        // for (int i = 0; i < drawBlockCount[0]; i++)
        // {
        //     Debug.Log("Draw Block Index : " + drawBlockIndex[i].x);
        //     Debug.Log("Block Vertex Start : " + drawBlockIndex[i].y);
        //     Debug.Log("Block Tris Start : " + drawBlockIndex[i].z);
        // }

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

        if (countsBuff != null) countsBuff.Release();

        if (meshVsBuff != null) meshVsBuff.Release();
        if (meshUVsBuff != null) meshUVsBuff.Release();
        if (meshTrisBuff != null) meshTrisBuff.Release();
        
        if (sourceMeshVsBuff != null) sourceMeshVsBuff.Release();
        if (sourceMeshUVsBuff != null) sourceMeshUVsBuff.Release();
        if (sourceMeshTrisBuff != null) sourceMeshTrisBuff.Release();
    }
}
