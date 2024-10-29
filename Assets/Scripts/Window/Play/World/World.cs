using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    // ���[���h���̃��X�g
    public static List<WorldInfo> WorldInfos;

    // ���݂̃��[���h���
    private WorldInfo currentWorldInfo;

    // �G���e�B�e�B�A�A�C�e���̎w��u���b�N�ɋ���VaxelId�̎���
    // Key�ɂ�3�����u���b�N���W������AValue�ɂ�VaxelId�̃��X�g������
    private Dictionary<int, List<int>> entitiesIDToVaxelIDs;
    private Dictionary<int, List<int>> itemsIDToVaxelIDs;

    // ���[���h�f�[�^�ƌ��т��eVaxelID�B�����l��0
    private int[,,] blocksID;
    private int[,,] entitiesID;
    private int[,,] itemsID;

    // ���[���h�f�[�^
    [SerializeField] private WorldData data;

    // �v���C���[
    [SerializeField] private Player player;

    private void Init()
    {
        // ID�ϊ������̏�����
        entitiesIDToVaxelIDs = new Dictionary<int, List<int>>();
        itemsIDToVaxelIDs = new Dictionary<int, List<int>>();

        // ���[���h�̏�����
        blocksID = new int[Constants.WORLD_SIZE, Constants.WORLD_HEIGHT, Constants.WORLD_SIZE];
        entitiesID = new int[Constants.WORLD_SIZE, Constants.WORLD_HEIGHT, Constants.WORLD_SIZE];
        itemsID = new int[Constants.WORLD_SIZE, Constants.WORLD_HEIGHT, Constants.WORLD_SIZE];

        data.Init(ref blocksID, ref entitiesID, ref itemsID);
    }

    public void Create()
    {
        // Param�ɕۑ�����Ă��郏�[���h�����g�p���ă��[���h�̐���

        // ���[���h�̏�����
        Init();

        // ���[���h�̐���
        data.Create(ref blocksID);

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
    }
}
