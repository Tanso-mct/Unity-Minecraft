using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    // ワールド情報のリスト
    public static List<WorldInfo> WorldInfos;

    // 現在のワールド情報
    private WorldInfo currentWorldInfo;

    // ブロック、エンティティ、アイテムのIDとインデックスの対応
    private Dictionary<int, int> entitiesIdToIndex;
    private Dictionary<int, int> itemsIdToIndex;

    // 現在のワールドデータと結びつく各タイプのデータ
    private Block[,,] blocks;
    private Entity[,,] entities;
    private Item[,,] items;

    // ワールドデータ
    [SerializeField] private WorldData data;

    private void WorldInit()
    {
        // ワールドの初期化
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
        // Paramに保存されているワールド情報を使用してワールドの生成

        // ワールドの初期化
        //WorldInit();
    }

    public void LoadFromJson()
    {
        // Paramに保存されているワールド情報から指定のJsonファイルを使用して読み込む

        // ワールドの初期化
        WorldInit();
    }

    public void SaveToJson()
    {
        // 現在のワールド情報、データをJsonファイルに保存
    }

    public void DestoroyJson()
    {
        // Paramに保存されているワールド情報を使用してワールドを削除
    }

    public static void LoadInfoFromJson()
    {
        WorldInfos = new List<WorldInfo>();

        // JSONファイルから読み込む
    }

    public void Execute()
    {
        Debug.Log("World Execute");
    }
}
