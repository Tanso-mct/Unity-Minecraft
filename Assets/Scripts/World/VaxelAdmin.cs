using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaxelAdmin
{
    private BlockAdmin blockAdmin;
    private ItemAdmin itemAdmin;
    private EntityAdmin entityAdmin;

    public void Init()
    {
        blockAdmin = new BlockAdmin();
        blockAdmin.Init();

        itemAdmin = new ItemAdmin();
        itemAdmin.Init();

        entityAdmin = new EntityAdmin();
        entityAdmin.Init();
    }

    public void LoadFromJson()
    {

    }

    public void SaveToJson()
    {

    }

    public void DestroyJson()
    {

    }
}
