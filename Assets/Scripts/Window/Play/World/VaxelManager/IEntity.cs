using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntity
{
    void Create(ref Vector3 coords, ref EntityManager entityMgr, GameObject vaxelObj);
}
