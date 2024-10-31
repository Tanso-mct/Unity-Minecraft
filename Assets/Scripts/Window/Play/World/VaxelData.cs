using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class VaxelData
{
    protected string prefabPath;
    protected GameObject vaxelObj;

    abstract public void Create(ref Vector3 coords, ref BlockManager blockMgr, GameObject vaxelObj);
    abstract public void Create(ref Vector3 coords, ref ItemManager itemMgr, GameObject vaxelObj);
    abstract public void Create(ref Vector3 coords, ref EntityManager entityMgr, GameObject vaxelObj);
}
