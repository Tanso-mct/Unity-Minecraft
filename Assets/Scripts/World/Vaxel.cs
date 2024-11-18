using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Vaxel
{
    protected int id;
    protected Container sourceSetContainer;
    protected Container sourceBreakContainer;

    abstract public void Init();
    abstract public void LoadFromJson(ref WorldData worldData);
    abstract public void SaveToJson(ref WorldData worldData);
}
