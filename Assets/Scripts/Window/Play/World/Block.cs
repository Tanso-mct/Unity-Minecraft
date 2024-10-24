using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private WorldData worldData;
    public int vaxelId = Constants.BLOCK_STATE_NULL;

    public Block(WorldData worldData)
    {
        this.worldData = worldData;
    }
}
