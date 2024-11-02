using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassData : VaxelData, IBlock, IItem, IEntity
{
    private GameObject blockPrefab;
    private GameObject blockObj;

    private Texture blockTopTexture;
    private Texture blockSideTexture;
    private Texture blockBottomTexture;

    void IBlock.Create(ref Vector3 coords, ref BlockManager blockMgr, GameObject vaxelObj)
    {
        // プレハブを読み込み作成
        ResourceInstantiatePrefab(ref blockPrefab, Constants.PREFAB_BLOCK, ref vaxelObj, ref blockObj, coords);

        // テクスチャを設定
        List<GameObject> children = GetChildren(blockObj);
        for (int i = 0; i < children.Count; i++)
        {
            MeshRenderer meshRenderer = children[i].GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                switch (children[i].name)
                {
                    case Constants.BLOCK_OBJ_TOP:
                        SetObjectTexture(ref meshRenderer, ref blockTopTexture);
                        break;

                    case Constants.BLOCK_OBJ_FRONT:
                    case Constants.BLOCK_OBJ_BACK:
                    case Constants.BLOCK_OBJ_LEFT:
                    case Constants.BLOCK_OBJ_RIGHT:
                        SetObjectTexture(ref meshRenderer, ref blockSideTexture);
                        break;

                    case Constants.BLOCK_OBJ_BOTTOM:
                        SetObjectTexture(ref meshRenderer, ref blockBottomTexture);
                        break;
                }
            }
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

    void IBlock.LoadTexture(List<string> texturePaths)
    {
        for (int i = 0; i < texturePaths.Count; i++)
        {
            switch (i)
            {
                case Constants.TEXTURE_FACE_TOP:
                    ResourceTextureLoad(ref blockTopTexture, texturePaths[i]);
                    break;

                case Constants.TEXTURE_FACE_BOTTOM:
                    ResourceTextureLoad(ref blockBottomTexture, texturePaths[i]);
                    break;

                case Constants.TEXTURE_FACE_SIDE:
                    ResourceTextureLoad(ref blockSideTexture, texturePaths[i]);
                    break;
            }
        }
    }
}
