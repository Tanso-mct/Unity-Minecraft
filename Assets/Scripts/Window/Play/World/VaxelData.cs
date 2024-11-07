using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class VaxelData
{
    [HideInInspector] public GameObject vaxelObj;

    protected List<GameObject> GetChildren(GameObject parent)
    {
        List<GameObject> children = new List<GameObject>();

        foreach (Transform child in parent.transform)
        {
            children.Add(child.gameObject);
        }

        return children;
    }

    protected void ResourceInstantiatePrefab
    (
        ref GameObject prefab, string prefabPath, ref GameObject parent, Vector3 coords
    ){
        prefab = Resources.Load<GameObject>(prefabPath);

        if (prefab != null)
        {
            vaxelObj = GameObject.Instantiate(prefab, coords, Quaternion.identity);

            if (vaxelObj != null)
            {
                vaxelObj.transform.SetParent(parent.transform);
            }
            else
            {
                Debug.LogError("Failed to instantiate prefab: " + prefabPath);
            }
        }
        else
        {
            Debug.LogError("Failed to load prefab: " + prefabPath);
        }
    }

    protected void ResourceTextureLoad(ref Texture texture, string texturePath)
    {
        texture = Resources.Load<Texture>(texturePath);
        if (texture == null) Debug.LogError("Failed to load texture: " + texturePath);
    }

    protected void SetObjectTexture(ref MeshRenderer meshRend, ref Texture texture)
    {
        if (meshRend != null) meshRend.material.mainTexture = texture;
        else Debug.LogError("Failed to set texture: " + texture.name);
    }
    
}
