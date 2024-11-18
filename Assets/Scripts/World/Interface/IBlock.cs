using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBlock
{
    float GetDurability();

    bool IsUseable();
    void Use(Vector4 block);

    void TrySet(Vector4 block, ref List<Vector4> frameSetBlock, ref Container sourceContainer);
    void TryBreak(Vector4 block, ref List<Vector4> frameDestroyBlock, ref Container sourceContainer);

    void FinishedSet(Vector4 block);
    void FinishedBreak(Vector4 block);
}
