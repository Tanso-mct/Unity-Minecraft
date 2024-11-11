using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMesh : MonoBehaviour
{
    // Combineしたメッシュの頂点、UV、頂点インデックス
    private Vector3[] vertices;
    private Vector2[] uv;
    private int[] triangles;

    public Texture2D textureAtlas;

    public void Init()
    {
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();

        CombineSquares();

        gameObject.SetActive(false);
    }

    public void CombineSquares()
    {
        // 新しいメッシュを作成
        Mesh combinedMesh = new Mesh();

        // CombineするSquareを設定
        List<GameObject> squaresToCombine = SupportFunc.GetChildren(gameObject);

        // CombineするSquareがない場合は処理を終了
        if (squaresToCombine.Count == 0)　return;

        // CombineするSquareのテクスチャタイルを設定
        List<Vector2Int> textureTiles = new List<Vector2Int>();
        for (int i = 0; i < squaresToCombine.Count; i++)
        {
            textureTiles.Add(squaresToCombine[i].GetComponent<WorldSquare>().textureTile);
        }

        // 各SquareのUVを設定
        for (int i = 0; i < squaresToCombine.Count; i++)
        {
            Vector2Int tile = new Vector2Int
            (
                textureTiles[i].x % 5, 
                (textureAtlas.height / 16 - 1) - (int)(textureTiles[i].y / 5)
            );

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
        }

        CombineInstance[] combine = new CombineInstance[squaresToCombine.Count];
        for (int i = 0; i < squaresToCombine.Count; i++)
        {
            combine[i].mesh = squaresToCombine[i].GetComponent<MeshFilter>().sharedMesh;
            combine[i].transform = squaresToCombine[i].transform.localToWorldMatrix;
        }

        combinedMesh.CombineMeshes(combine);

        // 結合したメッシュを適用
        GetComponent<MeshFilter>().sharedMesh = combinedMesh;
        GetComponent<MeshRenderer>().material.mainTexture = textureAtlas;

        // Combineしたメッシュの頂点、UV、頂点インデックスを取得
        vertices = combinedMesh.vertices;
        uv = combinedMesh.uv;
        triangles = combinedMesh.triangles;

        // 最大頂点数、頂点インデックスを設定
        Constants.SOURCE_MESH_VS_MAX = (vertices.Length > Constants.SOURCE_MESH_VS_MAX) ? vertices.Length : Constants.SOURCE_MESH_VS_MAX;
        Constants.SOURCE_MESH_TRIS_MAX = (triangles.Length > Constants.SOURCE_MESH_TRIS_MAX) ? triangles.Length : Constants.SOURCE_MESH_TRIS_MAX;
    }

    public void SetData(ref ComputeShader shader, ref List<Vector3> vertices, ref List<Vector2> uv, ref List<int> triangles)
    {
        shader.SetInt("SOURCE_MESH_BLOCK_VS_INDEX", vertices.Count);
        shader.SetInt("SOURCE_MESH_BLOCK_TRIS_INDEX", triangles.Count);
        
        shader.SetInt("SOURCE_MESH_BLOCK_VS_SIZE", this.vertices.Length);
        shader.SetInt("SOURCE_MESH_BLOCK_TRIS_SIZE", this.triangles.Length);

        vertices.AddRange(this.vertices);
        uv.AddRange(this.uv);
        triangles.AddRange(this.triangles);
    }
}
