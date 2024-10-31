using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreData : VaxelData, IBlock, IItem, IEntity
{
    private GameObject blockPrefab;
    private Texture blockTexture;

    public void LoadTexture(List<string> texturePaths)
    {
        this.blockTexture = Resources.Load<Texture>(texturePaths[Constants.TEXTURE_FACE_FULL]);
        if (this.blockTexture == null)
        {
            Debug.LogError("Failed to load texture: " + texturePaths[0]);
        }
    }

    public void Create(ref Vector3 coords, ref BlockManager blockMgr, GameObject vaxelObj)
    {
        // プレハブを読み込む
        GameObject prefab = Resources.Load<GameObject>(Constants.PREFAB_BLOCK);

        if (prefab != null)
        {
            // 親オブジェクトを保存
            this.vaxelObj = vaxelObj;

            // プレハブをインスタンス化
            GameObject blockObj = GameObject.Instantiate(prefab, coords, Quaternion.identity);

            // ブロックのゲームオブジェクトをデータのゲームオブジェクトの子にする
            blockObj.transform.SetParent(vaxelObj.transform);

            // テクスチャを設定
            List<GameObject> children = GetChildren(blockObj);
            for (int i = 0; i < children.Count; i++)
            {
                MeshRenderer meshRenderer = children[i].GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                {
                    meshRenderer.material.mainTexture = blockTexture;
                }
            }

            // ブロックのゲームオブジェクトを登録
            blockMgr.Register(vaxelObj.GetComponent<Vaxel>().data as IBlock);    
        }
    }

    public void Create(ref Vector3 coords, ref ItemManager itemMgr, GameObject vaxelObj)
    {
        
    }

    public void Create(ref Vector3 coords, ref EntityManager entityMgr, GameObject vaxelObj)
    {
        
    }
}
