using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    // ���[���h���̃��X�g
    public static List<WorldInfo> WorldInfos;

    // ���݂̃��[���h���
    private WorldInfo currentWorldInfo;

    // �u���b�N�A�G���e�B�e�B�A�A�C�e����ID�ƃC���f�b�N�X�̑Ή�
    private Dictionary<int, int> entitiesIdToIndex;
    private Dictionary<int, int> itemsIdToIndex;

    // ���݂̃��[���h�f�[�^�ƌ��т��e�^�C�v�̃f�[�^
    private Block[,,] blocks;
    private Entity[,,] entities;
    private Item[,,] items;

    // ���[���h�f�[�^
    [SerializeField] private WorldData data;

    private void WorldInit()
    {
        // ���[���h�̏�����
        blocks = new Block[Constants.WORLD_SIZE, Constants.WORLD_HEIGHT, Constants.WORLD_SIZE];
        entities = new Entity[Constants.WORLD_SIZE, Constants.WORLD_HEIGHT, Constants.WORLD_SIZE];
        items = new Item[Constants.WORLD_SIZE, Constants.WORLD_HEIGHT, Constants.WORLD_SIZE];
        for (int i = 0; i < Constants.WORLD_SIZE; i++)
        {
        }

        //Parallel.For(0, Constants.WORLD_SIZE, x =>
        //{
        //    for (int y = 0; y < Constants.WORLD_HEIGHT; y++)
        //    {
        //        for (int z = 0; z < Constants.WORLD_SIZE; z++)
        //        {
        //            blocks[x, y, z] = new Block(data);
        //            entities[x, y, z] = new Entity(data);
        //            items[x, y, z] = new Item(data);
        //        }
        //    }
        //});

        data.Init(blocks, entities, items);
    }

    public void Create()
    {
        // Param�ɕۑ�����Ă��郏�[���h�����g�p���ă��[���h�̐���

        // ���[���h�̏�����
        //WorldInit();
    }

    public void LoadFromJson()
    {
        // Param�ɕۑ�����Ă��郏�[���h��񂩂�w���Json�t�@�C�����g�p���ēǂݍ���

        // ���[���h�̏�����
        WorldInit();
    }

    public void SaveToJson()
    {
        // ���݂̃��[���h���A�f�[�^��Json�t�@�C���ɕۑ�
    }

    public void DestoroyJson()
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
        Debug.Log("World Execute");
    }
}
