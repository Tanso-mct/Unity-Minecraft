using System.Collections;
using System.Collections.Generic;
using UnityEditor.iOS;
using UnityEngine;

public class BuildBlockData : VaxelData, IBlock, IItem, IEntity
{
    private GameObject blockPrefab;
    private Texture blockTexture;

    void IBlock.LoadTexture(List<string> texturePaths)
    {
        ResourceTextureLoad(ref blockTexture, texturePaths[Constants.TEXTURE_FACE_FULL]);
    }

    void IBlock.Create(ref Vector3 coords, ref BlockManager blockMgr, GameObject parent)
    {
        // プレハブを読み込み作成
        ResourceInstantiatePrefab(ref blockPrefab, Constants.PREFAB_BLOCK, ref parent, coords);

        // テクスチャを設定
        List<GameObject> children = GetChildren(vaxelObj);
        for (int i = 0; i < children.Count; i++)
        {
            MeshRenderer meshRenderer = children[i].GetComponent<MeshRenderer>();
            SetObjectTexture(ref meshRenderer, ref blockTexture);
        }

        // ブロックのゲームオブジェクトを登録
        blockMgr.Register(vaxelObj.GetComponent<Vaxel>().data as IBlock);    
    }

    void IItem.Create(ref Vector3 coords, ref ItemManager itemMgr, GameObject vaxelObj)
    {
        
    }

    void IEntity.Create(ref Vector3 coords, ref EntityManager entityMgr, GameObject vaxelObj)
    {
        
    }

}
