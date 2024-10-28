using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldData : MonoBehaviour
{
    public LinkedList<Vaxel> vaxels;

    public void Init(ref int[,,] blocksID, ref int[,,] entitiesID, ref int[,,] itemsID)
    {
        vaxels = new LinkedList<Vaxel>();
    }

    public void Create(ref int[,,] blocksID)
    {
        Debug.Log("Create World");
    }

    public void SpawnMob(ref int[,,] entitiesID)
    {
        Debug.Log("Spawn Mob");
    }

    public void Load(ref WorldInfo info)
    {

    }
}
