using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class VaxelData
{
    protected GameObject vaxelObj;

    protected List<GameObject> GetChildren(GameObject parent)
    {
        List<GameObject> children = new List<GameObject>();

        foreach (Transform child in parent.transform)
        {
            children.Add(child.gameObject);
        }

        return children;
    }
    
}
