using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

abstract public class Element : MonoBehaviour
{
    // エレメントの種類を設定
    [SerializeField] private string type = "";
    public string Type { get { return type; } }

    // 要素の外枠になる画像を持つGameObjectを設定。画像は透明度０にすることで非表示状態にしておく
    [SerializeField] protected GameObject frame;

    // 使用する画像の親オブジェクトを設定
    [SerializeField] private GameObject images;

    // 使用する画像を格納
    private List<Image> liImages;

    // エレメントの表示状態を設定。
    private bool isShow = false;
    public bool IsShow { get { return isShow; } set { isShow = value; } }

    // 画像らの初期化処理を記述
    protected void InitImages()
    {
        liImages = new List<Image>();
        foreach (Transform child in images.transform)
        {
            child.gameObject.GetComponent<Image>().enabled = false;
            liImages.Add(child.gameObject.GetComponent<Image>());
        }

        // フレームの透明度を０にすることで非表示状態にする
        frame.GetComponent<Image>().color = new Color(0, 0, 0, 0);
    }

    // Imageの表示状態を設定
    protected void ShowImages(bool val)
    {
        for (int i = 0; i < liImages.Count; i++)
        {
            liImages[i].enabled = val;
        }
    }

    // エレメントの初期化処理を記述。初期化時にエレメントは非表示状態にする
    abstract public void Init();

    // エレメントの表示処理を記述
    abstract public void Show();

    // エレメントの非表示処理を記述
    abstract public void Close();

    // エレメントの実行処理を記述
    abstract public void Execute();

    // エレメントの移動処理を記述
    public void Move(ref Vector2 vec)
    {
        for (int i = 0; i < liImages.Count; i++)
        {
            Vector2 newVec = new Vector2
            (
                liImages[i].rectTransform.anchoredPosition.x + vec.x,
                liImages[i].rectTransform.anchoredPosition.y + vec.y
            );
            liImages[i].rectTransform.anchoredPosition = newVec;
        }
    }

    public bool IsClick()
    {
        return false;
    }
}
