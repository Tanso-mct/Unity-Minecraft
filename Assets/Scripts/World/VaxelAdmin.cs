using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaxelAdmin
{
    private BlockAdmin blockAdmin;
    private ItemAdmin itemAdmin;

    public void Init()
    {
        blockAdmin = new BlockAdmin();
        blockAdmin.Init();

        itemAdmin = new ItemAdmin();
        itemAdmin.Init();
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
