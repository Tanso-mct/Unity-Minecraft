using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMesh : MonoBehaviour
{
    // Combineしたメッシュの頂点、UV、頂点インデックス
    private List<Vector3> vertices;
    private List<Vector2> uv;
    private List<int> triangles;

    public Texture2D textureAtlas;

    List<GameObject> squares;

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

        // Squareを設定
        squares = SupportFunc.GetChildren(gameObject);

        // Squareがない場合は処理を終了
        if (squares.Count == 0)　return;

        // Squareのテクスチャタイルを設定
        List<Vector2Int> textureTiles = new List<Vector2Int>();
        for (int i = 0; i < squares.Count; i++)
        {
            textureTiles.Add(squares[i].GetComponent<WorldSquare>().textureTile);
        }

        // 各SquareのUVを設定
        for (int i = 0; i < squares.Count; i++)
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

            Mesh mesh = squares[i].GetComponent<MeshFilter>().mesh;
            squares[i].GetComponent<MeshRenderer>().material.mainTexture = textureAtlas;

            Vector2[] uvs = new Vector2[4];

            uvs[0] = new Vector2(originUV.x, originUV.y);
            uvs[1] = new Vector2(oppositeUV.x, originUV.y); 
            uvs[2] = new Vector2(originUV.x, oppositeUV.y);
            uvs[3] = new Vector2(oppositeUV.x, oppositeUV.y);
            
            mesh.uv = uvs;
        }

        // 各Squareの情報を格納
        vertices = new List<Vector3>();
        uv = new List<Vector2>();
        triangles = new List<int>();
        for (int i = 0; i < squares.Count; i++)
        {
            vertices.AddRange(squares[i].GetComponent<MeshFilter>().mesh.vertices);
            uv.AddRange(squares[i].GetComponent<MeshFilter>().mesh.uv);
            triangles.AddRange(squares[i].GetComponent<MeshFilter>().mesh.triangles);
        }

        // 最大頂点数、頂点インデックスを設定
        Constants.SOURCE_MESH_VS_MAX = (vertices.Count > Constants.SOURCE_MESH_VS_MAX) ? vertices.Count : Constants.SOURCE_MESH_VS_MAX;
        Constants.SOURCE_MESH_TRIS_MAX = (triangles.Count > Constants.SOURCE_MESH_TRIS_MAX) ? triangles.Count : Constants.SOURCE_MESH_TRIS_MAX;
    }

    public void SetData(ref ComputeShader shader, ref List<Vector3> vertices, ref List<Vector2> uv, ref List<int> triangles)
    {
        shader.SetInt("SOURCE_MESH_BLOCK_FRONT", 0);
        shader.SetInt("SOURCE_MESH_BLOCK_BACK", 1);
        shader.SetInt("SOURCE_MESH_BLOCK_LEFT", 2);
        shader.SetInt("SOURCE_MESH_BLOCK_RIGHT", 3);
        shader.SetInt("SOURCE_MESH_BLOCK_TOP", 4);
        shader.SetInt("SOURCE_MESH_BLOCK_BOTTOM", 5);

        shader.SetInt("SOURCE_MESH_BLOCK_VS_INDEX", vertices.Count);
        shader.SetInt("SOURCE_MESH_BLOCK_TRIS_INDEX", triangles.Count);

        shader.SetInt("SOURCE_MESH_BLOCK_FACE_COUNT", squares.Count);
        
        vertices.AddRange(this.vertices);
        uv.AddRange(this.uv);
        triangles.AddRange(this.triangles);
    }
}
