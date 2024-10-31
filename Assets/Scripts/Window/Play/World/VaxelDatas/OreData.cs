using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreData : VaxelData, IBlock, IItem, IEntity
{
    public override void Create(ref Vector3 coords, ref BlockManager blockMgr, GameObject vaxelObj)
    {
        // プレハブを読み込む
        GameObject prefab = Resources.Load<GameObject>(Constants.PREFAB_BLOCK);

        if (prefab != null)
        {
            // プレハブのパスを保存
            prefabPath = Constants.PREFAB_BLOCK;

            // 親オブジェクトを保存
            this.vaxelObj = vaxelObj;

            // プレハブをインスタンス化
            GameObject blockObj = GameObject.Instantiate(prefab, coords, Quaternion.identity);

            // ブロックのゲームオブジェクトをデータのゲームオブジェクトの子にする
            blockObj.transform.SetParent(vaxelObj.transform);

            // ブロックのゲームオブジェクトを登録
            blockMgr.Register(vaxelObj.GetComponent<Vaxel>().data as IBlock);    
        }
    }

    public override void Create(ref Vector3 coords, ref ItemManager itemMgr, GameObject vaxelObj)
    {
        
    }

    public override void Create(ref Vector3 coords, ref EntityManager entityMgr, GameObject vaxelObj)
    {
        
    }
}
