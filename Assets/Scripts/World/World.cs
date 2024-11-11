using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    // ���[���h���̃��X�g
    public static List<WorldInfo> WorldInfos;

    // ���݂̃��[���h���
    private static WorldInfo thisInfo;

    // �u���b�N�̃o�b�t�@�[�B���̍��W�ɑ��݂��Ă���u���b�N�̎�ނ������B
    private ComputeBuffer blocksIDBuff;

    // ���[���h��̃u���b�N�ƃG���e�B�e�B�ƃA�C�e���̃I�u�W�F�N�g
    Dictionary<Vector3Int, Vaxel> blocks;
    List<Vaxel> entities;
    List<Vaxel> items;

    // ���[���h���b�V���I�u�W�F�N�g
    [SerializeField] private GameObject objWorldMesh;
    private Mesh worldMesh;

    // �eVaxel�̃\�[�X���b�V���I�u�W�F�N�g
    [SerializeField] private WorldMesh meshBlock;

    // �v���C���[
    [SerializeField] private Player player;

    // Vaxel�̊Ǘ��N���X
    private VaxelAdmin vaxelAdmin;

    // Shader
    [SerializeField] private ComputeShader worldShader;

    // [0] �`�悷��u���b�N�̐� [1][2][3] �u���b�N�̃C���f�b�N�X�A���_�̊J�n�ʒu�A���_�C���f�b�N�X�̊J�n�ʒu
    private ComputeBuffer countsBuff;
    private int drawBlockCount = 0;
    private int meshVsCount = 0;
    private int meshTrisCount = 0;


    // ���[���h���b�V���̒��_�f�[�^�o�b�t�@�[�B�`��͈͂ɂ��T�C�Y���ς��B
    private ComputeBuffer meshVsBuff;

    // ���[���h���b�V����UV�f�[�^�o�b�t�@�[�B�`��͈͂ɂ��T�C�Y���ς��B
    private ComputeBuffer meshUVsBuff;

    // ���[���h���b�V���̒��_�C���f�b�N�X�f�[�^�o�b�t�@�[�B�`��͈͂ɂ��T�C�Y���ς��B
    private ComputeBuffer meshTrisBuff;

    // �\�[�X�u���b�N��̃��b�V���f�[�^
    List<Vector3> sourceMeshVs;
    List<Vector2> sourceMeshUVs;
    List<int> sourceMeshTris;
    private ComputeBuffer sourceMeshVsBuff;
    private ComputeBuffer sourceMeshUVsBuff;
    private ComputeBuffer sourceMeshTrisBuff;

    public static void LoadInfoFromJson()
    {
        WorldInfos = new List<WorldInfo>();

        // �ǂݍ���
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
        // JSON�t�@�C���ɕۑ�
    }

    public void Init()
    {
        // ���[���h�̏�����
        blocksIDBuff = new ComputeBuffer(Constants.WORLD_SIZE * Constants.WORLD_HEIGHT * Constants.WORLD_SIZE, sizeof(int));

        blocks = new Dictionary<Vector3Int, Vaxel>();
        entities = new List<Vaxel>();
        items = new List<Vaxel>();

        // Vaxel�̊Ǘ��N���X�̏�����
        vaxelAdmin = new VaxelAdmin();
        vaxelAdmin.Init();

        // �\�[�X���b�V���I�u�W�F�N�g�̏�����
        sourceMeshVs = new List<Vector3>();
        sourceMeshUVs = new List<Vector2>();
        sourceMeshTris = new List<int>();
        
        meshBlock.Init();

        // �\�[�X���b�V���I�u�W�F�N�g�̃f�[�^���쐬
        meshBlock.SetData(ref worldShader, ref sourceMeshVs, ref sourceMeshUVs, ref sourceMeshTris);

        // �\�[�X���b�V���I�u�W�F�N�g�̃o�b�t�@�[�쐬
        sourceMeshVsBuff = new ComputeBuffer(sourceMeshVs.Count, sizeof(float) * 3);
        sourceMeshUVsBuff = new ComputeBuffer(sourceMeshUVs.Count, sizeof(float) * 2);
        sourceMeshTrisBuff = new ComputeBuffer(sourceMeshTris.Count, sizeof(int));

        sourceMeshVsBuff.SetData(sourceMeshVs);
        sourceMeshUVsBuff.SetData(sourceMeshUVs);
        sourceMeshTrisBuff.SetData(sourceMeshTris);

        // ���[���h���b�V���̏�����
        worldMesh = new Mesh();
        objWorldMesh.GetComponent<MeshFilter>().mesh = worldMesh;

        Texture meshAtlasTexture = null;
        SupportFunc.LoadTexture(ref meshAtlasTexture, Constants.TEXTURE_ATLAS_BLOCK);
        objWorldMesh.GetComponent<MeshRenderer>().material.mainTexture = meshAtlasTexture;

        // [0] �`�悷��u���b�N�̐� [1][2] ���_�̊J�n�ʒu�A���_�C���f�b�N�X�̊J�n�ʒu�������o�b�t�@�[
        countsBuff = new ComputeBuffer(3, sizeof(int));

        int[] countsAry = new int[3];
        for (int i = 0; i < 3; i++) countsAry[i] = 0;
        countsBuff.SetData(countsAry);
        
        // ���[���h���b�V���̒��_�AUV�A���_�C���f�b�N�X�̃o�b�t�@�[�쐬
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

        // ���[���h�̐����������͓ǂݍ���
        if(thisInfo.dataJsonPath == "") Create(thisInfo.worldType);
        else LoadFromJson();
    }

    public void SetGenerateBuffer(int kernelIndex)
    {
        // �V�F�[�_�[�̒萔���Z�b�g
        Constants.SetShaderConstants(ref worldShader);

        // �e�o�b�t�@�[���V�F�[�_�[�ɃZ�b�g
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
        // �V�F�[�_�[�̒萔���Z�b�g
        Constants.SetShaderConstants(ref worldShader);

        // �e�o�b�t�@�[���V�F�[�_�[�ɃZ�b�g
        worldShader.SetBuffer(kernelIndex, "blocksID", blocksIDBuff);

        worldShader.SetInt("RENDER_DISTANCE", McVideos.RenderDistance);
    }

    // Param�ɕۑ�����Ă��郏�[���h�����g�p���ă��[���h�̐���
    public void Create(string worldType)
    {
        // �v���C���[�̏�����
        player.Init();

        // ���[���h�S�̂��������邽�߂̃X���b�h�O���[�v��
        int worldThreadGroupsX = Mathf.CeilToInt(Constants.WORLD_SIZE / 8.0f);
        int worldThreadGroupsY = Mathf.CeilToInt(Constants.WORLD_HEIGHT / 8.0f);
        int worldThreadGroupsZ = Mathf.CeilToInt(Constants.WORLD_SIZE / 8.0f);

        // �v���C���[�̍��W��ϊ�
        Vector3Int convertedPos = SupportFunc.CoordsIntConvert(player.Pos);

        // �`��͈͂��������邽�߂̃X���b�h�O���[�v��
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

        // ��������C���f�b�N�X�̊J�n�n�_�A�I���n�_���Z�b�g
        worldShader.SetInt("VIEW_ORIGIN_X", worldOrigin.x);
        worldShader.SetInt("VIEW_ORIGIN_Y", worldOrigin.y);
        worldShader.SetInt("VIEW_ORIGIN_Z", worldOrigin.z);

        if (thisInfo.worldType == "Flat")
        {
            // �t���b�g���[���h�̐���
            int generateFlatWorld = worldShader.FindKernel("GenerateFlatWorld");
            SetGenerateBuffer(generateFlatWorld);
            worldShader.Dispatch(generateFlatWorld, worldThreadGroupsX, worldThreadGroupsY, worldThreadGroupsZ);
        }
        else 
        {
            // �_�C�A�����h�X�N�G�A�A���S���Y���ɂ�郏�[���h�̐���
            int diamondSquareStep = worldShader.FindKernel("DiamondSquareStep");
            worldShader.Dispatch(diamondSquareStep, worldThreadGroupsX, worldThreadGroupsY, worldThreadGroupsZ);
        }

        // ��C�Ɨאڂ���u���b�N��`��u���b�N�Ƃ��A�����̏����擾
        int meshGenerate = worldShader.FindKernel("MeshGenerate");
        SetGenerateBuffer(meshGenerate);
        worldShader.Dispatch(meshGenerate, viewThreadGroupsX, viewThreadGroupsY, viewThreadGroupsZ);

        // �e�����擾
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

        // �v���C���[�̐����y�єz�u
        player.Init();
        player.Create();
    }

    // Param�ɕۑ�����Ă��郏�[���h��񂩂�w���Json�t�@�C�����g�p���ēǂݍ���
    public void LoadFromJson()
    {
        // ���b�V���̓ǂݍ���

        // �v���C���[�̓ǂݍ��݋y�єz�u
        player.Init();
        player.LoadFromJson();
    }

    public void SaveToJson()
    {
        // ���݂̃��[���h���A�f�[�^��Json�t�@�C���ɕۑ�
    }

    public void DestroyJson()
    {
        // Param�ɕۑ�����Ă��郏�[���h�����g�p���ă��[���h���폜
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
        // �v���C���[�̎��s
        player.Execute();

        // ���b�V���̍X�V
        MeshUpdate();

        // �u���b�N�̍X�V
        BlockUpdate();

        // �G���e�B�e�B�̍X�V
        EntityUpdate();

        // �A�C�e���̍X�V
        ItemUpdate();
    }

    void OnDestroy()
    {
        // �o�b�t�@�����
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
