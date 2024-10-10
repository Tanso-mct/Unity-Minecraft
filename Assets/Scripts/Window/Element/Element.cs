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

    // 要素に使用した画像らをインスペクターで設定。
    [SerializeField] private List<Image> images;

    // エレメントの表示状態を設定。
    private bool isShow = false;
    public bool IsShow { get { return isShow; } set { isShow = value; } }

    // Imageの表示状態を設定
    protected void ShowImages(bool val)
    {
        for (int i = 0; i < images.Count; i++)
        {
            images[i].enabled = val;
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
        foreach (var image in images)
        {
            Vector2 newVec = new Vector2
            (
                image.rectTransform.anchoredPosition.x + vec.x,
                image.rectTransform.anchoredPosition.y + vec.y
            );
            image.rectTransform.anchoredPosition = newVec;
        }
    }
}
