using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

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

    // �v���C���[
    [SerializeField] private Player player;

    // Vaxel�̊Ǘ��N���X
    private VaxelAdmin vaxelAdmin;

    // Shader
    [SerializeField] private ComputeShader worldShader;

    // �eVaxel�̃\�[�X���b�V���I�u�W�F�N�g
    [SerializeField] private WorldMesh meshBlock;
    [SerializeField] private WorldMesh meshGrass;
    [SerializeField] private WorldMesh meshStairs;

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

    // Raycast�p�̃o�b�t�@�[
    private ComputeBuffer raycastBlocksBuff;
    Vector4[] raycastBlocks;

    // Raycast�̐��x
    [SerializeField] private int rayAccuracy = 100;

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
        meshGrass.Init();
        meshStairs.Init();

        // �\�[�X���b�V���I�u�W�F�N�g�̃f�[�^���쐬
        meshBlock.SetData(ref worldShader, ref sourceMeshVs, ref sourceMeshUVs, ref sourceMeshTris);
        meshGrass.SetGrassData(ref worldShader, ref sourceMeshVs, ref sourceMeshUVs, ref sourceMeshTris);
        meshStairs.SetStairsData(ref worldShader, ref sourceMeshVs, ref sourceMeshUVs, ref sourceMeshTris);

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
        worldMesh.MarkDynamic();

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

        // Raycast�̏�����
        RaycastInit();

        // ���[���h�̐����������͓ǂݍ���
        if(thisInfo.dataJsonPath == "") Create(thisInfo.worldType);
        else LoadFromJson();
    }

    public void SetBuffer(int kernelIndex)
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

        // ��������C���f�b�N�X�̊J�n�n�_�A�I���n�_���Z�b�g
        worldShader.SetInt("VIEW_ORIGIN_X", worldOrigin.x);
        worldShader.SetInt("VIEW_ORIGIN_Y", worldOrigin.y);
        worldShader.SetInt("VIEW_ORIGIN_Z", worldOrigin.z);

        int[] blocksId = new int[Constants.WORLD_SIZE * Constants.WORLD_HEIGHT * Constants.WORLD_SIZE];
        if (thisInfo.worldType == "Flat")
        {
            // �t���b�g���[���h�̐���
            for (int x = 0; x < Constants.WORLD_SIZE; x++)
            {
                for (int y = 0; y < Constants.WORLD_HEIGHT; y++)
                {
                    for (int z = 0; z < Constants.WORLD_SIZE; z++)
                    {
                        int index = x + y * Constants.WORLD_SIZE + z * Constants.WORLD_SIZE * Constants.WORLD_HEIGHT;
                        if (y == 0) blocksId[index] = (int)Constants.BLOCK_TYPE.BEDROCK; 
                        else if (y >= 1 && y <= 2) blocksId[index] = (int)Constants.BLOCK_TYPE.DIRT;
                        else if (y == 3) blocksId[index] = (int)Constants.BLOCK_TYPE.GRASS_TOP;
                        else blocksId[index] = (int)Constants.BLOCK_TYPE.AIR;
                    }
                }
            }

            // �����̒��𐶐�
            for (int x = Constants.WORLD_HALF_SIZE + 1; x < Constants.WORLD_HALF_SIZE + 10; x++)
            {
                int y = 5;
                int z = Constants.WORLD_HALF_SIZE;
                int index = x + y * Constants.WORLD_SIZE + z * Constants.WORLD_SIZE * Constants.WORLD_HEIGHT;
                blocksId[index] = (int)Constants.BLOCK_TYPE.BEDROCK;
            }
        }
        else 
        {
            // �_�C�A�����h�X�N�G�A�A���S���Y���ɂ�郏�[���h�̐���
            
        }

        blocksIDBuff.SetData(blocksId);

        int viewThreadGroupsX = Mathf.CeilToInt((worldOpposite.x - worldOrigin.x) / 8.0f);
        int viewThreadGroupsY = Mathf.CeilToInt((worldOpposite.y - worldOrigin.y) / 8.0f);
        int viewThreadGroupsZ = Mathf.CeilToInt((worldOpposite.z - worldOrigin.z) / 8.0f);

        // ��C�Ɨאڂ���u���b�N��`��u���b�N�Ƃ��A�����̏����擾
        int meshGenerate = worldShader.FindKernel("MeshGenerate");
        SetBuffer(meshGenerate);
        worldShader.Dispatch(meshGenerate, viewThreadGroupsX, viewThreadGroupsY, viewThreadGroupsZ);

        // �e�����擾
        int[] countsAry = new int[3];
        countsBuff.GetData(countsAry);

        drawBlockCount = countsAry[0];
        meshVsCount = countsAry[1];
        meshTrisCount = countsAry[2];

        // �J�E���g�z���������
        for (int i = 0; i < 3; i++) countsAry[i] = 0;
        countsBuff.SetData(countsAry);

        // ���[���h���b�V���̒��_�AUV�A���_�C���f�b�N�X���擾
        Vector3[] meshVsAry = new Vector3[meshVsCount];
        Vector2[] meshUVsAry = new Vector2[meshVsCount];
        int[] meshTrisAry = new int[meshTrisCount];

        meshVsBuff.GetData(meshVsAry);
        meshUVsBuff.GetData(meshUVsAry);
        meshTrisBuff.GetData(meshTrisAry);

        // ���[���h���b�V���̒��_�AUV�A���_�C���f�b�N�X��ݒ�
        worldMesh.Clear();
        worldMesh.SetVertices(meshVsAry);
        worldMesh.SetUVs(0, meshUVsAry);
        worldMesh.SetTriangles(meshTrisAry, 0);

        // ���[���h���b�V���̍X�V
        worldMesh.RecalculateTangents();
        worldMesh.RecalculateNormals();
        worldMesh.RecalculateBounds();
        worldMesh.Optimize();

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

    private void RaycastInit()
    {
        int raySize = (int)(player.Reach * rayAccuracy);

        // ���C�L���X�g�p�̃o�b�t�@�[���쐬
        raycastBlocks = new Vector4[raySize];
        for (int i = 0; i < raySize; i++)
        {
            raycastBlocks[i].x = 0;
            raycastBlocks[i].y = 0;
            raycastBlocks[i].z = 0;
            raycastBlocks[i].w = 0;
        }

        raycastBlocksBuff = new ComputeBuffer(raySize, sizeof(float) * 4);
        raycastBlocksBuff.SetData(raycastBlocks);
    }

    // X = Block.x, Y = Block.y, Z = Block.z W = BlockType
    private List<Vector4> RaycastAtBlock()
    {
        Vector3 origin = player.Pos;
        origin.x += Constants.WORLD_HALF_SIZE;
        origin.y += 1.75f;
        origin.z += Constants.WORLD_HALF_SIZE;

        Vector3 direction = player.cam.transform.forward;

        int raySize = (int)(player.Reach * rayAccuracy);

        worldShader.SetFloat("RAY_SIZE", raySize);

        worldShader.SetFloat("RAY_ORIGIN_X", origin.x);
        worldShader.SetFloat("RAY_ORIGIN_Y", origin.y);
        worldShader.SetFloat("RAY_ORIGIN_Z", origin.z);

        worldShader.SetFloat("RAY_DIRECTION_X", direction.x);
        worldShader.SetFloat("RAY_DIRECTION_Y", direction.y);
        worldShader.SetFloat("RAY_DIRECTION_Z", direction.z);

        worldShader.SetFloat("RAY_LENGTH", player.Reach);

        int raycastAtBlock = worldShader.FindKernel("RaycastAtBlock");

        // �V�F�[�_�[�̒萔���Z�b�g
        Constants.SetShaderConstants(ref worldShader);

        worldShader.SetBuffer(raycastAtBlock, "blocksID", blocksIDBuff);
        worldShader.SetBuffer(raycastAtBlock, "raycastBlocks", raycastBlocksBuff);

        int threadGroupsX = Mathf.CeilToInt(raySize / 8.0f);
        int threadGroupsY = Mathf.CeilToInt(1);
        int threadGroupsZ = Mathf.CeilToInt(1);

        worldShader.Dispatch(raycastAtBlock, threadGroupsX, threadGroupsY, threadGroupsZ);

        raycastBlocksBuff.GetData(raycastBlocks);

        Vector4 selectBlock = new Vector4(0, 0, 0, 0);
        Vector4 setBlock = new Vector4(0, 0, 0, 0);
        for (int i = 0; i < raycastBlocks.Length; i++)
        {
            if (raycastBlocks[i].w != 0f)
            {
                selectBlock.x = raycastBlocks[i].x;
                selectBlock.y = raycastBlocks[i].y;
                selectBlock.z = raycastBlocks[i].z;
                selectBlock.w = raycastBlocks[i].w;

                if (i - 1 >= 0)
                {
                    setBlock.x = raycastBlocks[i - 1].x;
                    setBlock.y = raycastBlocks[i - 1].y;
                    setBlock.z = raycastBlocks[i - 1].z;
                    setBlock.w = raycastBlocks[i - 1].w;
                }
                else
                {
                    setBlock.w = Constants.BLOCK_TYPE_CANT_SET;
                }
                break;
            }
        }

        return new List<Vector4> { selectBlock, setBlock };
    }

    private void MeshUpdate()
    {
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

        // ��C�Ɨאڂ���u���b�N��`��u���b�N�Ƃ��A�����̏����擾
        int meshGenerate = worldShader.FindKernel("MeshGenerate");
        SetBuffer(meshGenerate);
        worldShader.Dispatch(meshGenerate, viewThreadGroupsX, viewThreadGroupsY, viewThreadGroupsZ);

        // �e�����擾
        int[] countsAry = new int[3];
        countsBuff.GetData(countsAry);

        drawBlockCount = countsAry[0];
        meshVsCount = countsAry[1];
        meshTrisCount = countsAry[2];

        // �J�E���g�z���������
        for (int i = 0; i < 3; i++) countsAry[i] = 0;
        countsBuff.SetData(countsAry);

        // ���[���h���b�V���̒��_�AUV�A���_�C���f�b�N�X���擾
        Vector3[] meshVsAry = new Vector3[meshVsCount];
        Vector2[] meshUVsAry = new Vector2[meshVsCount];
        int[] meshTrisAry = new int[meshTrisCount];

        meshVsBuff.GetData(meshVsAry);
        meshUVsBuff.GetData(meshUVsAry);
        meshTrisBuff.GetData(meshTrisAry);

        // ���[���h���b�V���̒��_�AUV�A���_�C���f�b�N�X��ݒ�
        worldMesh.Clear();
        worldMesh.SetVertices(meshVsAry);
        worldMesh.SetUVs(0, meshUVsAry);
        worldMesh.SetTriangles(meshTrisAry, 0);

        // ���[���h���b�V���̍X�V
        worldMesh.RecalculateTangents();
        worldMesh.RecalculateNormals();
        worldMesh.RecalculateBounds();
        worldMesh.Optimize();

    }

    private void BlockUpdate()
    {
        // �u���b�N�̐���
        int blockUpdate = worldShader.FindKernel("BlockUpdate");
        worldShader.SetBuffer(blockUpdate, "blocksID", blocksIDBuff);

        // �V�F�[�_�[�̒萔���Z�b�g
        Constants.SetShaderConstants(ref worldShader);

        for (int i = 0; i < player.frameSetBlocks.Count; i++)
        {
            worldShader.SetInt("TARGET_BLOCK_X", (int)player.frameSetBlocks[i].x);
            worldShader.SetInt("TARGET_BLOCK_Y", (int)player.frameSetBlocks[i].y);
            worldShader.SetInt("TARGET_BLOCK_Z", (int)player.frameSetBlocks[i].z);
            worldShader.SetInt("GENERATE_BLOCK_ID", (int)player.frameSetBlocks[i].w);
            worldShader.Dispatch(blockUpdate, 1, 1, 1);
        }
        player.frameSetBlocks.Clear();

        // �u���b�N�̍폜
        for (int i = 0; i < player.frameDestroyBlocks.Count; i++)
        {
            worldShader.SetInt("TARGET_BLOCK_X", (int)player.frameDestroyBlocks[i].x);
            worldShader.SetInt("TARGET_BLOCK_Y", (int)player.frameDestroyBlocks[i].y);
            worldShader.SetInt("TARGET_BLOCK_Z", (int)player.frameDestroyBlocks[i].z);
            worldShader.SetInt("GENERATE_BLOCK_ID", (int)Constants.BLOCK_TYPE.AIR);
            worldShader.Dispatch(blockUpdate, 1, 1, 1);
        }
        player.frameDestroyBlocks.Clear();
    }

    private void EntityUpdate()
    {

    }

    private void ItemUpdate()
    {

    }

    public void Execute()
    {
        // �v���C���[�̎��s�BRaycast�̌��ʂ𑗐M
        player.Execute(RaycastAtBlock());

        // �u���b�N�̍X�V
        BlockUpdate();

        // ���b�V���̍X�V
        MeshUpdate();

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

        if (raycastBlocksBuff != null) raycastBlocksBuff.Release();
    }
}
