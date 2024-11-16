using System.Collections.Generic;
using UnityEngine;

public class SquareCombiner : MonoBehaviour
{
    public GameObject[] squaresToCombine1;
    public GameObject[] squaresToCombine2;

    public Texture2D textureAtlas;

    List<GameObject> children;

    void Start()
    {
        children = new List<GameObject>();
        children.Add(new GameObject("CombinedSquare"));
        children.Add(new GameObject("CombinedSquare"));

        for (int i = 0; i < children.Count; i++)
        {
            children[i].transform.parent = transform;
            children[i].AddComponent<MeshFilter>();
            children[i].AddComponent<MeshRenderer>();
        }

        CombineSquares(0, ref squaresToCombine1);
        CombineSquares(1, ref squaresToCombine2);
    }

    void CombineSquares(int meshIndex, ref GameObject[] squaresToCombine)
    {
        // 新しいメッシュを作成
        Mesh combinedMesh = new Mesh();
        
        CombineInstance[] combine = new CombineInstance[squaresToCombine.Length];

        // 各SquareのUVを設定
        for (int i = 0; i < squaresToCombine.Length; i++)
        {
            Vector2Int tile = new Vector2Int(0 % 5, (textureAtlas.height / 16 - 1) - (int)(0 / 5));

            Vector2 originUV = new Vector2(tile.x * 16, tile.y * 16);
            originUV.x /= textureAtlas.width;
            originUV.y /= textureAtlas.height;

            Vector2 oppositeUV = new Vector2(tile.x * 16 + 16, tile.y * 16 + 16);
            oppositeUV.x /= textureAtlas.width;
            oppositeUV.y /= textureAtlas.height;

            Mesh mesh = squaresToCombine[i].GetComponent<MeshFilter>().mesh;
            squaresToCombine[i].GetComponent<MeshRenderer>().material.mainTexture = textureAtlas;

            Vector2[] uvs = new Vector2[4];

            uvs[0] = new Vector2(originUV.x, originUV.y);
            uvs[1] = new Vector2(oppositeUV.x, originUV.y); 
            uvs[2] = new Vector2(originUV.x, oppositeUV.y);
            uvs[3] = new Vector2(oppositeUV.x, oppositeUV.y);
            
            mesh.uv = uvs;

            Debug.Log("Name: " + squaresToCombine[i].name);
            Debug.Log("UVs: " + mesh.uv.Length);
            for (int j = 0; j < mesh.uv.Length; j++)
            {
                Debug.Log("UV " + j + ": " + mesh.uv[j]);
            }

        }

        for (int i = 0; i < squaresToCombine.Length; i++)
        {
            combine[i].mesh = squaresToCombine[i].GetComponent<MeshFilter>().sharedMesh;
            combine[i].transform = squaresToCombine[i].transform.localToWorldMatrix;
        }

        combinedMesh.CombineMeshes(combine);

        // 結合したメッシュを適用
        MeshFilter meshFilter = children[meshIndex].GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = children[meshIndex].GetComponent<MeshRenderer>();

        meshFilter.mesh = combinedMesh;
        meshRenderer.material.mainTexture = textureAtlas;

        // 元のSquareを非表示にする
        foreach (GameObject square in squaresToCombine)
        {
            square.SetActive(false);
        }

        // Combineしたメッシュの頂点、UV、頂点インデックスを取得
        Vector3[] vertices = combinedMesh.vertices;
        Vector2[] uv = combinedMesh.uv;
        int[] triangles = combinedMesh.triangles;

        // // デバッグ用にログ出力
        // Debug.Log("Vertices: " + vertices.Length);
        // for (int i = 0; i < vertices.Length; i++)
        // {
        //     Debug.Log("Vertex " + i + ": " + vertices[i]);
        // }
        
        // Debug.Log("UVs: " + uv.Length);
        // for (int i = 0; i < uv.Length; i++)
        // {
        //     Debug.Log("UV " + i + ": " + uv[i]);
        // }

        // Debug.Log("Triangles: " + triangles.Length);
        // for (int i = 0; i < triangles.Length; i++)
        // {
        //     Debug.Log("Triangle " + i + ": " + triangles[i]);
        // }

        // メッシュを移動させる
        // Vector3 offset = new Vector3(2.0f, 0.0f, 0.0f); // 移動量を設定
        // for (int i = 0; i < vertices.Length; i++)
        // {
        //     vertices[i] += offset;
        // }
        // combinedMesh.vertices = vertices;
        
        combinedMesh.RecalculateNormals(); // 法線を再計算
        combinedMesh.RecalculateTangents(); // 接線を再計算
        combinedMesh.Optimize();
    }
}