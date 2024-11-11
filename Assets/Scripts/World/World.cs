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

    // �`�悷��u���b�N�̐�
    private int[] drawBlockCount;
    private ComputeBuffer drawBlockCountBuff;

    // �`�悷��u���b�N�̃C���f�b�N�X�ԍ��̔z��By, z, w�͂��ꂼ�꒸�_�AUV�A���_�C���f�b�N�X�̏������݂͂��߈ʒu�B
    private ComputeBuffer drawBlockIndexBuff;

    // ���b�V���̒��_�AUV�A���_�C���f�b�N�X�̂��ꂼ��̍��v���B
    private int meshVsCount = 0;
    private int meshUVsCount = 0;
    private int meshTrisCount = 0;

    private ComputeBuffer meshVsCountBuff;
    private ComputeBuffer meshUVsCountBuff;
    private ComputeBuffer meshTrisCountBuff;

    // ���[���h���b�V���̒��_�f�[�^�o�b�t�@�[�B�`��͈͂ɂ��T�C�Y���ς��B
    private ComputeBuffer meshVsBuff;

    // ���[���h���b�V����UV�f�[�^�o�b�t�@�[�B�`��͈͂ɂ��T�C�Y���ς��B
    private ComputeBuffer meshUVsBuff;

    // ���[���h���b�V���̒��_�C���f�b�N�X�f�[�^�o�b�t�@�[�B�`��͈͂ɂ��T�C�Y���ς��B
    private ComputeBuffer meshTrisBuff;

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

        // ���[���h���b�V���̏�����
        worldMesh = new Mesh();
        objWorldMesh.GetComponent<MeshFilter>().mesh = worldMesh;

        Texture meshAtlasTexture = null;
        SupportFunc.LoadTexture(ref meshAtlasTexture, Constants.TEXTURE_ATLAS_BLOCK);
        objWorldMesh.GetComponent<MeshRenderer>().material.mainTexture = meshAtlasTexture;

        // �\�[�X���b�V���I�u�W�F�N�g�̏�����
        meshBlock.Init();

        // �\�[�X���b�V���I�u�W�F�N�g�̃o�b�t�@�[�쐬
        meshBlock.CreateBuffer();

        // �u���b�N�̕`�搔�̃o�b�t�@�[�쐬
        drawBlockCountBuff = new ComputeBuffer(1, sizeof(int));
        drawBlockCount = new int[1];
        drawBlockCount[0] = 0;
        drawBlockCountBuff.SetData(drawBlockCount);

        // �u���b�N�̏��̃o�b�t�@�[�쐬
        drawBlockIndexBuff = new ComputeBuffer
        (
            McVideos.RenderDistance * Constants.CHUCK_SIZE * 
            McVideos.RenderDistance * Constants.CHUCK_SIZE * 
            McVideos.RenderDistance * Constants.CHUCK_SIZE, 
            sizeof(int) * 4
        );

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

        // ���b�V���̒��_�AUV�A���_�C���f�b�N�X�̐��̃o�b�t�@�[�쐬
        int[] countAry = new int[1];
        countAry[0] = 0;
        meshVsCountBuff = new ComputeBuffer(1, sizeof(int));
        meshVsCountBuff.SetData(countAry);

        meshUVsCountBuff = new ComputeBuffer(1, sizeof(int));
        meshUVsCountBuff.SetData(countAry);

        meshTrisCountBuff = new ComputeBuffer(1, sizeof(int));
        meshTrisCountBuff.SetData(countAry);

        // ���[���h�̐����������͓ǂݍ���
        if(thisInfo.dataJsonPath == "") Create(thisInfo.worldType);
        else LoadFromJson();
    }

    // Param�ɕۑ�����Ă��郏�[���h�����g�p���ă��[���h�̐���
    public void Create(string worldType)
    {
        // ���b�V���̐���
        int threadGroupsX = Mathf.CeilToInt(Constants.WORLD_SIZE / 8.0f);
        int threadGroupsY = Mathf.CeilToInt(Constants.WORLD_HEIGHT / 8.0f);
        int threadGroupsZ = Mathf.CeilToInt(Constants.WORLD_SIZE / 8.0f);

        // �V�F�[�_�[�̒萔���Z�b�g
        Constants.SetShaderConstants(ref worldShader);

        // �e�o�b�t�@�[���V�F�[�_�[�ɃZ�b�g
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
            // �t���b�g���[���h�̐���
            int generateFlatWorld = worldShader.FindKernel("GenerateFlatWorld");
            worldShader.Dispatch(generateFlatWorld, threadGroupsX, threadGroupsY, threadGroupsZ);
            
            int getBlocksAdjacentAir = worldShader.FindKernel("GetBlocksAdjacentAir");
            worldShader.Dispatch(getBlocksAdjacentAir, threadGroupsX, threadGroupsY, threadGroupsZ);
        }
        else 
        {
            // �_�C�A�����h�X�N�G�A�A���S���Y���ɂ�郏�[���h�̐���
            int diamondSquareStep = worldShader.FindKernel("DiamondSquareStep");
            worldShader.Dispatch(diamondSquareStep, threadGroupsX, threadGroupsY, threadGroupsZ);
        }

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
        
        meshBlock.ReleaseBuffer();
    }
}
