using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportFunc
{
    static public Vector3Int CoordsIntConvert(Vector3 coords)
    {
        Vector3Int rtCoords = new Vector3Int();

        rtCoords.x = (int)coords.x + Constants.WORLD_HALF_SIZE;
        rtCoords.y = (int)coords.y;
        rtCoords.z = (int)coords.z + Constants.WORLD_HALF_SIZE;

        return rtCoords;
    }

    static public Vector3 CoordsFloatConvert(Vector3 coords)
    {
        Vector3 rtCoords = new Vector3();       

        rtCoords.x = coords.x + Constants.WORLD_HALF_SIZE;
        rtCoords.y = coords.y;
        rtCoords.z = coords.z + Constants.WORLD_HALF_SIZE;

        return rtCoords;
    }

    static public List<GameObject> GetChildren(GameObject parent)
    {
        List<GameObject> children = new List<GameObject>();

        foreach (Transform child in parent.transform)
        {
            children.Add(child.gameObject);
        }

        return children;
    }

    static public void InstantiatePrefab
    (
        ref GameObject target, ref GameObject prefab, string prefabPath, ref GameObject parent, Vector3 coords
    ){
        prefab = Resources.Load<GameObject>(prefabPath);

        if (prefab != null)
        {
            target = GameObject.Instantiate(prefab, coords, Quaternion.identity);

            if (target != null)
            {
                target.transform.SetParent(parent.transform);
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

    static public void LoadTexture(ref Texture texture, string texturePath)
    {
        texture = Resources.Load<Texture>(texturePath);
        if (texture == null) Debug.LogError("Failed to load texture: " + texturePath);
    }

    static public void SetMeshRendererTexture(ref MeshRenderer meshRend, ref Texture texture)
    {
        if (meshRend != null) meshRend.material.mainTexture = texture;
        else Debug.LogError("Failed to set texture: " + texture.name);
    }

}
