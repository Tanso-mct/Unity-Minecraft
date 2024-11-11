using UnityEngine;

public class SquareCombiner : MonoBehaviour
{
    public GameObject[] squaresToCombine;

    public Texture2D textureAtlas;

    void Start()
    {
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();

        CombineSquares();
    }

    void CombineSquares()
    {
        // 新しいメッシュを作成
        // Mesh combinedMesh = new Mesh();
        
        // CombineInstance[] combine = new CombineInstance[squaresToCombine.Length];

        // 各SquareのUVを設定
        for (int i = 0; i < squaresToCombine.Length; i++)
        {
            Mesh mesh = squaresToCombine[i].GetComponent<MeshFilter>().mesh;
            squaresToCombine[i].GetComponent<MeshRenderer>().material.mainTexture = textureAtlas;
            Vector2[] uvs = new Vector2[4];

            Vector2 originUV = new Vector2((int)(i % 5) * 16, 32 - (int)(i / 5) * 16);
            originUV.x /= textureAtlas.width;
            originUV.y /= textureAtlas.height;

            Vector2 oppositeUV = new Vector2((int)(i % 5) * 16 + 16, 32 - (int)(i / 5) * 16 + 16);
            oppositeUV.x /= textureAtlas.width;
            oppositeUV.y /= textureAtlas.height;

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

            // Mesh mesh = squaresToCombine[i].GetComponent<MeshFilter>().mesh;
            // squaresToCombine[i].GetComponent<MeshRenderer>().material.mainTexture = textureAtlas;
            // Vector2[] uvs = new Vector2[mesh.vertexCount];

            // // テクスチャアトラスのサイズ
            // float atlasWidth = 80.0f;
            // float atlasHeight = 32.0f;

            // // ブロックのサイズ
            // float blockWidth = 16.0f;
            // float blockHeight = 16.0f;

            // // 左上のブロックのUV座標を計算
            // Vector2 originUV = new Vector2(0, atlasHeight - blockHeight);
            // originUV.x /= atlasWidth;
            // originUV.y /= atlasHeight;

            // Vector2 oppositeUV = new Vector2(blockWidth, atlasHeight);
            // oppositeUV.x /= atlasWidth;
            // oppositeUV.y /= atlasHeight;

            // // UV座標を設定
            // uvs[0] = new Vector2(originUV.x, originUV.y);
            // uvs[1] = new Vector2(oppositeUV.x, originUV.y);
            // uvs[2] = new Vector2(originUV.x, oppositeUV.y);
            // uvs[3] = new Vector2(oppositeUV.x, oppositeUV.y);

            // mesh.uv = uvs;

            // Debug.Log("Name: " + squaresToCombine[i].name);
            // Debug.Log("UVs: " + mesh.uv.Length);
            // for (int j = 0; j < mesh.uv.Length; j++)
            // {
            //     Debug.Log("UV " + j + ": " + mesh.uv[j]);
            // }
        }

        // for (int i = 0; i < squaresToCombine.Length; i++)
        // {
        //     combine[i].mesh = squaresToCombine[i].GetComponent<MeshFilter>().sharedMesh;
        //     combine[i].transform = squaresToCombine[i].transform.localToWorldMatrix;
        // }

        // combinedMesh.CombineMeshes(combine);

        // // 結合したメッシュを適用
        // GetComponent<MeshFilter>().sharedMesh = combinedMesh;
        // GetComponent<MeshRenderer>().material.mainTexture = textureAtlas;

        // // 元のSquareを非表示にする
        // foreach (GameObject square in squaresToCombine)
        // {
        //     square.SetActive(false);
        // }

        // // Combineしたメッシュの頂点、UV、頂点インデックスを取得
        // Vector3[] vertices = combinedMesh.vertices;
        // Vector2[] uv = combinedMesh.uv;
        // int[] triangles = combinedMesh.triangles;

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
    }
}