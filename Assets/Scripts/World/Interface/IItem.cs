using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem
{
    float GetMiningSpeed(Constants.VAXEL_TYPE blockType);

    void UseItem(Camera cam, Vector4 target, ref List<Vector4> frameSetBlock);
    void DiscardItem(Vector3 playerPos, Vector3 playerDir);
}
