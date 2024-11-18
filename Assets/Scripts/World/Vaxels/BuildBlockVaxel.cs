using System.Collections;
using System.Collections.Generic;
using UnityEditor.iOS;
using UnityEngine;

public class BuildBlockVaxel : Vaxel
{
    public override float GetDurability()
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

    public override float GetMiningSpeed(Constants.VAXEL_TYPE blockType)
    {
        return Constants.MINING_SPEED_HAND;
    }

    public override void LoadFromJson(ref WorldData worldData)
    {
        
    }

    public override void SaveToJson(ref WorldData worldData)
    {
        
    }
}
