using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Vaxel
{
    abstract public void Init();
    abstract public void LoadFromJson(ref WorldData worldData);
    abstract public void SaveToJson(ref WorldData worldData);
}
