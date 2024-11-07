using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    // ���[���h���̃��X�g
    public static List<WorldInfo> WorldInfos;

    // ���݂̃��[���h���
    private WorldInfo currentWorldInfo;

    // ���[���h�f�[�^�ƌ��т��eVaxelID�B�����l��0�B���̍��W�u���b�N�ɑ��݂��Ă��邩�������B
    private int[,,] blocksID;
    private int[,,] entitiesID;
    private int[,,] itemsID;

    // ���[���h�f�[�^
    [SerializeField] private GameObject dataObj;
    private WorldData data;

    // �v���C���[
    [SerializeField] private Player player;

    // �e��`�Ԃ̃}�l�[�W���[
    private BlockManager blockMgr;
    private ItemManager itemMgr;
    private EntityManager entityMgr;

    private int debugNum = 0;


    private void Init()
    {
        // �f�[�^�̎擾
        data = dataObj.GetComponent<WorldData>();

        // ���[���h�̏�����
        blocksID = new int[Constants.WORLD_SIZE, Constants.WORLD_HEIGHT, Constants.WORLD_SIZE];
        entitiesID = new int[Constants.WORLD_SIZE, Constants.WORLD_HEIGHT, Constants.WORLD_SIZE];
        itemsID = new int[Constants.WORLD_SIZE, Constants.WORLD_HEIGHT, Constants.WORLD_SIZE];

        // �}�l�[�W���[�̏�����
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
        // Param�ɕۑ�����Ă��郏�[���h�����g�p���ă��[���h�̐���

        // ���[���h�̏�����
        Init();

        // ���[���h�̐���
        data.Create(dataObj, ref blocksID, ref entitiesID, ref itemsID, ref blockMgr, ref itemMgr, ref entityMgr);

        // ���u�̃X�|�[��
        data.SpawnMob(ref entitiesID);

        // �v���C���[�̐����y�єz�u
        player.Init();
        player.Create();
    }

    public void LoadFromJson()
    {
        // Param�ɕۑ�����Ă��郏�[���h��񂩂�w���Json�t�@�C�����g�p���ēǂݍ���

        // ���[���h�̏�����
        Init();
    }

    public void SaveToJson()
    {
        // ���݂̃��[���h���A�f�[�^��Json�t�@�C���ɕۑ�
    }

    public void DestroyJson()
    {
        // Param�ɕۑ�����Ă��郏�[���h�����g�p���ă��[���h���폜
    }

    public static void LoadInfoFromJson()
    {
        WorldInfos = new List<WorldInfo>();

        // JSON�t�@�C������ǂݍ���
    }

    public void Execute()
    {
        // �v���C���[�̎��s
        player.Execute();

        if (Input.GetKeyDown(KeyCode.F1))
        {
            // �f�o�b�O�p
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
