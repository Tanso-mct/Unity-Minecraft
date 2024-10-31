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

        Vector3 stoneCoords = new Vector3(0, 0, 0);
        CreateVaxel(Constants.VAXEL_TYPE.STONE, ref stoneCoords, ref blocksID, ref blockMgr, dataObj);
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
            // プレハブをインスタンス化
            GameObject newObj =  GameObject.Instantiate(prefab);

            // ブロックのゲームオブジェクトをデータのゲームオブジェクトの子にする
            newObj.transform.SetParent(dataObj.transform);

            return newObj.GetComponent<Vaxel>();
        }

        return null;
    }

    public void CreateVaxel(Constants.VAXEL_TYPE type, ref Vector3 coords, ref int[,,] blocks, ref BlockManager blockMgr, GameObject dataObj)
    {
        switch (type)
        {
        case Constants.VAXEL_TYPE.STONE:
            Vaxel vaxel = CreateVaxel(dataObj);
            
            vaxel.data = new OreData();
            vaxel.data.Create(ref coords, ref blockMgr, vaxel.gameObject);
            vaxels.AddLast(vaxel);

            Vector3 convertedCoords = CoordsIntConvert(coords);
            blocks[(int)convertedCoords.x, (int)convertedCoords.y, (int)convertedCoords.z] = vaxels.Count - 1;
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
