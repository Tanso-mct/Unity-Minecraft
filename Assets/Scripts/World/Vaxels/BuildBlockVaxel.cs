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
            case (int)Constants.VAXEL_TYPE.GRASS_TOP:
            case (int)Constants.VAXEL_TYPE.LEAVES:
                return 30;

            case (int)Constants.VAXEL_TYPE.LOG_OAK_TOP:
            case (int)Constants.VAXEL_TYPE.LOG_BIRCH_TOP:
            case (int)Constants.VAXEL_TYPE.PLANKS_OAK:
            case (int)Constants.VAXEL_TYPE.PLANKS_BIRCH:
                return 200;

            case (int)Constants.VAXEL_TYPE.STONE:
            case (int)Constants.VAXEL_TYPE.STONE_ANDESITE:
            case (int)Constants.VAXEL_TYPE.STONE_DIORITE:
            case (int)Constants.VAXEL_TYPE.STONE_GRANITE:
                return 300;

            case (int)Constants.VAXEL_TYPE.COBBLESTONE:
            case (int)Constants.VAXEL_TYPE.COAL_ORE:
            case (int)Constants.VAXEL_TYPE.IRON_ORE:
            case (int)Constants.VAXEL_TYPE.GOLD_ORE:
            case (int)Constants.VAXEL_TYPE.DIAMOND_ORE:
            case (int)Constants.VAXEL_TYPE.EMERALD_ORE:
                return 350;

            case (int)Constants.VAXEL_TYPE.OBSIDIAN:
                return 1000;
            
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
