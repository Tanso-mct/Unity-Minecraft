using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private WorldData worldData;
    private LinkedList<int> vaxelIds;

    public Item(WorldData worldData)
    {
        this.worldData = worldData;
        vaxelIds = new LinkedList<int>();
    }
}
