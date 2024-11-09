using UnityEngine;
using System.Collections.Generic;

public class MinecraftTerrain : MonoBehaviour
{
    public Texture2D textureAtlas;
    public int atlasSizeInBlocks = 16;
    public int tileSize = 16;

    private Dictionary<Vector3Int, int> blocks = new Dictionary<Vector3Int, int>();
    private Mesh mesh;
    private List<Vector3> vertices = new List<Vector3>();
    private List<Vector2> uv = new List<Vector2>();
    private List<int> triangles = new List<int>();

    int planes = 0;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().material.mainTexture = textureAtlas;

        // 初期ブロックの生成
        AddBlock(new Vector3Int(0, 0, 0), 0);
    }

    public void AddBlock(Vector3Int position, int textureIndex)
    {
        if (!blocks.ContainsKey(position))
        {
            blocks.Add(position, textureIndex);
            UpdateMesh();
        }
    }

    public void RemoveBlock(Vector3Int position)
    {
        if (blocks.ContainsKey(position))
        {
            blocks.Remove(position);
            UpdateMesh();
        }
    }

    private void UpdateMesh()
    {
        vertices.Clear();
        uv.Clear();
        triangles.Clear();

        foreach (var block in blocks)
        {
            AddBlockMesh(block.Key, block.Value);
        }

        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.uv = uv.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }

    private void AddBlockMesh(Vector3Int position, int textureIndex)
    {
        int vertexIndex = vertices.Count;

        // 頂点座標
        List<Vector3> blockVs;
        blockVs = new List<Vector3>();

        blockVs.Add(new Vector3(-0.5f, 0.5f, -0.5f));
        blockVs.Add(new Vector3(0.5f, 0.5f, -0.5f));
        blockVs.Add(new Vector3(0.5f, -0.5f, -0.5f));
        blockVs.Add(new Vector3(-0.5f, -0.5f, -0.5f));
        blockVs.Add(new Vector3(-0.5f, 0.5f, 0.5f));
        blockVs.Add(new Vector3(0.5f, 0.5f, 0.5f));
        blockVs.Add(new Vector3(0.5f, -0.5f, 0.5f));
        blockVs.Add(new Vector3(-0.5f, -0.5f, 0.5f));

        AddFace(blockVs, 0, 1, 2, 3, ref position, ref vertexIndex); // Front
        AddFace(blockVs, 5, 4, 7, 6, ref position, ref vertexIndex); // Back
        AddFace(blockVs, 4, 0, 3, 7, ref position, ref vertexIndex); // Left
        AddFace(blockVs, 1, 5, 6, 2, ref position, ref vertexIndex); // Right
        AddFace(blockVs, 4, 5, 1, 0, ref position, ref vertexIndex); // Top
        AddFace(blockVs, 3, 2, 6, 7, ref position, ref vertexIndex); // Bottom
    }

    private void AddFace(List<Vector3> blockVs, int v0, int v1, int v2, int v3, ref Vector3Int position, ref int vertexIndex)
    {
        vertices.Add(position + blockVs[v0]);
        vertices.Add(position + blockVs[v1]);
        vertices.Add(position + blockVs[v2]);
        vertices.Add(position + blockVs[v3]);

        // UV座標
        // Vector2[] blockUVs = CalculateUVs(textureIndex);
        // uv.AddRange(blockUVs);

        // 三角形のインデックス
        triangles.Add(vertexIndex + 0);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
        triangles.Add(vertexIndex + 0);
        triangles.Add(vertexIndex + 2);
        triangles.Add(vertexIndex + 3);

        vertexIndex += 4;
    }

    private Vector2[] CalculateUVs(int textureIndex)
    {
        float uvTileSize = 1.0f / atlasSizeInBlocks;
        int x = textureIndex % atlasSizeInBlocks;
        int y = textureIndex / atlasSizeInBlocks;

        Vector2[] uv = new Vector2[4];
        uv[0] = new Vector2(x * uvTileSize, y * uvTileSize);
        uv[1] = new Vector2((x + 1) * uvTileSize, y * uvTileSize);
        uv[2] = new Vector2((x + 1) * uvTileSize, (y + 1) * uvTileSize);
        uv[3] = new Vector2(x * uvTileSize, (y + 1) * uvTileSize);

        return uv;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            AddBlock(new Vector3Int(planes, 0, 0), 1); // 新しいブロックを生成
            planes++;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RemoveBlock(new Vector3Int(planes-1, 0, 0)); // ブロックを破棄
            planes--;
        }
    }
}