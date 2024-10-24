using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    private WorldData worldData;
    public LinkedList<int> vaxelIds; 

    public Entity(WorldData worldData)
    {
        this.worldData = worldData;
        vaxelIds = new LinkedList<int>();
    }
}
