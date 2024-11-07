using UnityEngine;

public class SquareCombiner : MonoBehaviour
{
    public GameObject[] squaresToCombine;

    void Start()
    {
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();

        CombineSquares();
    }

    void CombineSquares()
    {
        // 新しいメッシュを作成
        Mesh combinedMesh = new Mesh();
        
        CombineInstance[] combine = new CombineInstance[squaresToCombine.Length];

        for (int i = 0; i < squaresToCombine.Length; i++)
        {
            combine[i].mesh = squaresToCombine[i].GetComponent<MeshFilter>().sharedMesh;
            combine[i].transform = squaresToCombine[i].transform.localToWorldMatrix;
        }

        combinedMesh.CombineMeshes(combine);

        // 結合したメッシュを適用
        GetComponent<MeshFilter>().sharedMesh = combinedMesh;
        GetComponent<MeshRenderer>().material = squaresToCombine[0].GetComponent<MeshRenderer>().sharedMaterial;

        // 元のSquareを非表示にする
        foreach (GameObject square in squaresToCombine)
        {
            square.SetActive(false);
        }
    }
}