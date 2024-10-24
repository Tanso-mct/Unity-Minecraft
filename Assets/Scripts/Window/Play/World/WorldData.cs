using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldData : MonoBehaviour
{
    public LinkedList<Vaxel> vaxels;

    private Block[,,] blocks;
    private Entity[,,] entities;
    private Item[,,] items;

    public void Init(Block[,,] blocks, Entity[,,] entities, Item[,,] items)
    {
        vaxels = new LinkedList<Vaxel>();

        this.blocks = blocks;
        this.entities = entities;
        this.items = items;
    }

    public void Create(ref Block[,,] blocks)
    {
        
    }

    public void SpawnMob(ref Entity[,,] entities)
    {

    }

    public void Load(WorldInfo info)
    {

    }
}
