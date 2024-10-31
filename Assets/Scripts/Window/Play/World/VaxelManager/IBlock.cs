using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBlock
{
    // テクスチャをコンストラクタで読み込む。
    void LoadTexture(List<string> texturePaths);

    // ブロックの生成
    void Create(ref Vector3 coords, ref BlockManager blockMgr, GameObject vaxelObj);
}
