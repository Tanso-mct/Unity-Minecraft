using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldData : MonoBehaviour
{
    public LinkedList<Vaxel> vaxels;

    public void Init()
    {
        vaxels = new LinkedList<Vaxel>();
    }

    public void Create
    (
        GameObject dataObj,
        ref int[,,] blocksID, ref int[,,] entitiesID, ref int[,,] itemsID,
        ref BlockManager blockMgr, ref ItemManager itemMgr, ref EntityManager entityMgr
    ){
        Debug.Log("Create World");

        // CreateVaxel(Constants.VAXEL_TYPE.STONE, new Vector3(0, 0, 0), ref blocksID, ref blockMgr, dataObj);
        // CreateVaxel(Constants.VAXEL_TYPE.COBBLESTONE, new Vector3(1, 0, 0), ref blocksID, ref blockMgr, dataObj);

        // CreateVaxel(Constants.VAXEL_TYPE.COAL_ORE, new Vector3(2, 0, 0), ref blocksID, ref blockMgr, dataObj);
        // CreateVaxel(Constants.VAXEL_TYPE.IRON_ORE, new Vector3(3, 0, 0), ref blocksID, ref blockMgr, dataObj);
        // CreateVaxel(Constants.VAXEL_TYPE.GOLD_ORE, new Vector3(4, 0, 0), ref blocksID, ref blockMgr, dataObj);
        // CreateVaxel(Constants.VAXEL_TYPE.DIAMOND_ORE, new Vector3(5, 0, 0), ref blocksID, ref blockMgr, dataObj);
        // CreateVaxel(Constants.VAXEL_TYPE.EMERALD_ORE, new Vector3(6, 0, 0), ref blocksID, ref blockMgr, dataObj);
        // CreateVaxel(Constants.VAXEL_TYPE.REDSTONE_ORE, new Vector3(7, 0, 0), ref blocksID, ref blockMgr, dataObj);
        // CreateVaxel(Constants.VAXEL_TYPE.LAPIS_ORE, new Vector3(8, 0, 0), ref blocksID, ref blockMgr, dataObj);

        // CreateVaxel(Constants.VAXEL_TYPE.GRASS, new Vector3(0, 0, 2), ref blocksID, ref blockMgr, dataObj);
        // CreateVaxel(Constants.VAXEL_TYPE.DIRT, new Vector3(2, 0, 2), ref blocksID, ref blockMgr, dataObj);
        // CreateVaxel(Constants.VAXEL_TYPE.BEDROCK, new Vector3(4, 0, 2), ref blocksID, ref blockMgr, dataObj);

        // Create super flat world
        // for (int x = -McVideos.RenderDistance * Constants.CHUCK_SIZE; x < McVideos.RenderDistance * Constants.CHUCK_SIZE; x++)
        // {
        //     for (int z = -McVideos.RenderDistance * Constants.CHUCK_SIZE; z < McVideos.RenderDistance * Constants.CHUCK_SIZE; z++)
        //     {
        //         CreateVaxel(Constants.VAXEL_TYPE.BEDROCK, new Vector3(x, 0, z), ref blocksID, ref blockMgr, dataObj);
        //     }
        // }
    }

    private Vector3 CoordsIntConvert(Vector3 coords)
    {
        Vector3 rtCoords = new Vector3();

        rtCoords.x = (int)coords.x + Constants.WORLD_HALF_SIZE;
        rtCoords.y = (int)coords.y;
        rtCoords.z = (int)coords.z + Constants.WORLD_HALF_SIZE;

        return rtCoords;
    }

    private Vector3 CoordsFloatConvert(Vector3 coords)
    {
        Vector3 rtCoords = new Vector3();       

        rtCoords.x = coords.x + Constants.WORLD_HALF_SIZE;
        rtCoords.y = coords.y;
        rtCoords.z = coords.z + Constants.WORLD_HALF_SIZE;

        return rtCoords;
    }

    private Vaxel CreateVaxel(GameObject dataObj)
    {
        GameObject prefab = Resources.Load<GameObject>(Constants.PREFAB_VAXEL);

        if (prefab != null)
        {
            // �v���n�u���C���X�^���X��
            GameObject newObj =  GameObject.Instantiate(prefab);

            // �u���b�N�̃Q�[���I�u�W�F�N�g���f�[�^�̃Q�[���I�u�W�F�N�g�̎q�ɂ���
            newObj.transform.SetParent(dataObj.transform);

            return newObj.GetComponent<Vaxel>();
        }

        return null;
    }

    private void CreateGrass(ref Vector3 coords, ref int[,,] blocks, ref BlockManager blockMgr, GameObject dataObj)
    {
        // VaxelData��������
        VaxelData buildBlockData = new OreData();

        // �u���b�N�ɐݒ�
        IBlock block = buildBlockData as IBlock;

        // �e�N�X�`����ǂݍ���
        block.LoadTexture(new List<string>{Constants.TEXTURE_GRASS_TOP, Constants.TEXTURE_GRASS_BOTTOM, Constants.TEXTURE_GRASS_SIDE});

        // �f�[�^�𐶐�
        block.Create(ref coords, ref blockMgr, dataObj);

        // Vaxel�擾
        Vaxel vaxel = buildBlockData.vaxelObj.GetComponent<Vaxel>();

        // �f�[�^��o�^
        vaxel.data = buildBlockData;

        // Vaxel�����X�g�ɒǉ�
        vaxels.AddLast(vaxel);

        // �u���b�NID��ݒ�
        Vector3 convertedCoords = CoordsIntConvert(coords);
        blocks[(int)convertedCoords.x, (int)convertedCoords.y, (int)convertedCoords.z] = vaxels.Count - 1;
    }

    private void CreateOre(string texturePath, ref Vector3 coords, ref int[,,] blocks, ref BlockManager blockMgr, GameObject dataObj)
    {
        // VaxelData��������
        VaxelData buildBlockData = new OreData();

        // �u���b�N�ɐݒ�
        IBlock block = buildBlockData as IBlock;

        // �e�N�X�`����ǂݍ���
        block.LoadTexture(new List<string>{texturePath});

        // �f�[�^�𐶐�
        block.Create(ref coords, ref blockMgr, dataObj);

        // Vaxel�擾
        Vaxel vaxel = buildBlockData.vaxelObj.GetComponent<Vaxel>();

        // �f�[�^��o�^
        vaxel.data = buildBlockData;

        // Vaxel�����X�g�ɒǉ�
        vaxels.AddLast(vaxel);

        // �u���b�NID��ݒ�
        Vector3 convertedCoords = CoordsIntConvert(coords);
        blocks[(int)convertedCoords.x, (int)convertedCoords.y, (int)convertedCoords.z] = vaxels.Count - 1;
    }

    private void CreateBuildBlock(string texturePath, ref Vector3 coords, ref int[,,] blocks, ref BlockManager blockMgr, GameObject dataObj)
    {
        // VaxelData��������
        VaxelData buildBlockData = new BuildBlockData();

        // �u���b�N�ɐݒ�
        IBlock block = buildBlockData as IBlock;

        // �e�N�X�`����ǂݍ���
        block.LoadTexture(new List<string>{texturePath});

        // �f�[�^�𐶐�
        block.Create(ref coords, ref blockMgr, dataObj);

        // Vaxel�擾
        Vaxel vaxel = buildBlockData.vaxelObj.GetComponent<Vaxel>();

        // �f�[�^��o�^
        vaxel.data = buildBlockData;

        // Vaxel�����X�g�ɒǉ�
        vaxels.AddLast(vaxel);

        // �u���b�NID��ݒ�
        Vector3 convertedCoords = CoordsIntConvert(coords);
        blocks[(int)convertedCoords.x, (int)convertedCoords.y, (int)convertedCoords.z] = vaxels.Count - 1;
    }

    public void CreateVaxel(Constants.VAXEL_TYPE type, Vector3 coords, ref int[,,] blocks, ref BlockManager blockMgr, GameObject dataObj)
    {
        switch (type)
        {
        case Constants.VAXEL_TYPE.BEDROCK:
            CreateBuildBlock(Constants.TEXTURE_BEDROCK, ref coords, ref blocks, ref blockMgr, dataObj);
            break;

        case Constants.VAXEL_TYPE.GRASS:
            CreateGrass(ref coords, ref blocks, ref blockMgr, dataObj);
            break;

        case Constants.VAXEL_TYPE.DIRT:
            CreateBuildBlock(Constants.TEXTURE_DIRT, ref coords, ref blocks, ref blockMgr, dataObj);
            break;

        case Constants.VAXEL_TYPE.STONE:
            CreateOre(Constants.TEXTURE_STONE, ref coords, ref blocks, ref blockMgr, dataObj);
            break;

        case Constants.VAXEL_TYPE.COBBLESTONE:
            CreateOre(Constants.TEXTURE_COBBLESTONE, ref coords, ref blocks, ref blockMgr, dataObj);
            break;

        case Constants.VAXEL_TYPE.COAL_ORE:
            CreateOre(Constants.TEXTURE_COAL_ORE, ref coords, ref blocks, ref blockMgr, dataObj);
            break;

        case Constants.VAXEL_TYPE.IRON_ORE:
            CreateOre(Constants.TEXTURE_IRON_ORE, ref coords, ref blocks, ref blockMgr, dataObj);
            break;

        case Constants.VAXEL_TYPE.GOLD_ORE:
            CreateOre(Constants.TEXTURE_GOLD_ORE, ref coords, ref blocks, ref blockMgr, dataObj);
            break;

        case Constants.VAXEL_TYPE.DIAMOND_ORE:
            CreateOre(Constants.TEXTURE_DIAMOND_ORE, ref coords, ref blocks, ref blockMgr, dataObj);
            break;

        case Constants.VAXEL_TYPE.EMERALD_ORE:
            CreateOre(Constants.TEXTURE_EMERALD_ORE, ref coords, ref blocks, ref blockMgr, dataObj);
            break;

        case Constants.VAXEL_TYPE.REDSTONE_ORE:
            CreateOre(Constants.TEXTURE_REDSTONE_ORE, ref coords, ref blocks, ref blockMgr, dataObj);
            break;

        case Constants.VAXEL_TYPE.LAPIS_ORE:
            CreateOre(Constants.TEXTURE_LAPIS_ORE, ref coords, ref blocks, ref blockMgr, dataObj);
            break;

        default :
            Debug.LogError("Vaxel is not block");
            break;
        }
    }

    public void CreateVaxel(Constants.VAXEL_TYPE type, ref ItemManager itemMgr)
    {

    }

    public void CreateVaxel(Constants.VAXEL_TYPE type, ref EntityManager entityMgr)
    {

    }

    public void SpawnMob(ref int[,,] entitiesID)
    {
        Debug.Log("Spawn Mob");
    }

    public void Load(ref WorldInfo info)
    {

    }
}
