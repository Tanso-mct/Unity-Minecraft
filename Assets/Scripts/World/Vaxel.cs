using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Vaxel
{
    abstract public void Init();
    abstract public void LoadFromJson();
    abstract public void SaveToJson();

    public void DestroyJson()
    {

    }
}
