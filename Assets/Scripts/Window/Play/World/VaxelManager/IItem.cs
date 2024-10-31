using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem
{
　　 void Create(ref Vector3 coords, ref ItemManager itemMgr, GameObject vaxelObj);
}
