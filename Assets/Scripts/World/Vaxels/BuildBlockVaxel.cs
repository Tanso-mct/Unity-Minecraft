using System.Collections;
using System.Collections.Generic;
using UnityEditor.iOS;
using UnityEngine;

public class BuildBlockVaxel : Vaxel, IBlock, IItem
{
    public void DiscardItem(Vector3 playerPos, Vector3 playerDir)
    {
        Debug.Log("DiscardItem ID : " + id);
    }

    public void FinishedBreak(Vector4 block, ref List<Vector4> frameDestroyBlock)
    {
        throw new System.NotImplementedException();
    }

    public void FinishedSet(Vector4 block, ref List<Vector4> frameSetBlock)
    {
        throw new System.NotImplementedException();
    }

    public float GetDurability()
    {
        switch (id)
        {
            case (int)Constants.VAXEL_TYPE.BEDROCK:
                return Constants.BLOCK_TYPE_CANT_BREAK;

            case (int)Constants.VAXEL_TYPE.DIRT:
                return 10;

            case (int)Constants.VAXEL_TYPE.GRASS_TOP:
                return 10;

            case (int)Constants.VAXEL_TYPE.OBSIDIAN:
                return 100;
        }

        return Constants.BLOCK_TYPE_CANT_BREAK;
    }

    public float GetMiningSpeed(Constants.VAXEL_TYPE blockType)
    {
        return Constants.MINING_SPEED_HAND;
    }

    public override void Init()
    {
        
    }

    public bool IsUseable()
    {
        return false;
    }

    public override void LoadFromJson(ref WorldData worldData)
    {
        
    }

    public override void SaveToJson(ref WorldData worldData)
    {
        
    }

    public void TryBreak(Vector4 block, ref List<Vector4> frameDestroyBlock, ref Container sourceContainer)
    {
        sourceBreakContainer = sourceContainer;
        frameDestroyBlock.Add(block);
    }

    public void TrySet(Vector4 block, ref List<Vector4> frameSetBlock, ref Container sourceContainer)
    {
        sourceSetContainer = sourceContainer;
        frameSetBlock.Add(block);
    }

    public void UseBlock(Vector4 block)
    {
        // ブロックは使用対象とならない
    }

    public void UseItem(Camera cam, Vector4 target, ref List<Vector4> frameSetBlock)
    {
        // 使用する = ブロックを設置する
    }
}
