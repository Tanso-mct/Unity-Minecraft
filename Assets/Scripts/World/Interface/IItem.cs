using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem
{
    float GetMiningSpeed(Constants.BLOCK_TYPE blockType);

    void Use(Camera cam, Vector4 block, ref List<Vector4> frameSetBlock);
    void Discard(Vector3 playerPos, Vector3 playerDir);
}
