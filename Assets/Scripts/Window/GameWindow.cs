using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

abstract public class GameWindow : MonoBehaviour
{
    private bool isCreated = false;
    public bool IsCreated
        { get { return isCreated; } }

    private bool isOpening = false;
    public bool IsOpening
        { get { return isOpening; } }

    [SerializeField] private bool isScroll = false;
    public bool IsScroll
        { get { return isScroll; } }

    [SerializeField] private bool isPopUp = false;
    public bool IsPopUp
        { get { return isPopUp; } }

    [SerializeField] private List<Canvas> canvases;
    [SerializeField] private List<Image> backgrounds;

    [SerializeField] protected List<GameObject> images = null;
    [SerializeField] protected List<GameObject> texts = null;

    protected Dictionary<string, Element> diImageEl;
    protected Dictionary<string, Element> diTextEl;

    // 孫オブジェクトをすべて取得
    protected List<GameObject> GetAllGrandchildren(GameObject parent)
    {
        List<GameObject> grandchildren = new List<GameObject>();
        foreach (Transform child in parent.transform)
        {
            foreach (Transform grandchild in child)
            {
                grandchildren.Add(grandchild.gameObject);
            }
        }
        return grandchildren;
    }

    protected void GetElements(ref List<GameObject> parents, ref Dictionary<string, Element> diEl)
    {
        for (int i = 0; i < parents.Count; i++)
        {
            List<GameObject> grandChildren = GetAllGrandchildren(parents[i]);
            for (int j = 0; j < grandChildren.Count; j++)
            {
                if (!diEl.TryAdd(grandChildren[j].name, grandChildren[j].GetComponent<Element>()))
                {
                    Debug.LogError("Failed to add " + grandChildren[j].name + " to diEl.");
                }
            }
        }
    }

    protected void ElementsInit(ref Dictionary<string, Element> diEl)
    {
        foreach (KeyValuePair<string, Element> pair in diEl)
        {
            pair.Value.Init();
        }
    }

    protected void ShowCanvases(bool val)
    {
        for (int i = 0; i < canvases.Count; i++)
        {
            canvases[i].enabled = val;
        }
    }

    protected void ShowPanels(bool val)
    {
        for (int i = 0; i < backgrounds.Count; i++)
        {
            backgrounds[i].enabled = val;
        }
    }

    abstract public void Init();
    // {
    //     Debug.Log("Window Init [" + name + "]");

    //     // 各キャンバスとパネルを非表示にする
    //     ShowCanvases(false);
    //     ShowPanels(false);

    //     // 各辞書変数を初期化
    //     diImageEl = new Dictionary<string, Element>();
    //     diTextEl = new Dictionary<string, Element>();

    //     // 各要素を取得
    //     GetElements(ref images, ref diImageEl);
    //     GetElements(ref texts, ref diTextEl);

    //     // 各要素を初期化
    //     ElementsInit(ref diImageEl);
    //     ElementsInit(ref diTextEl);

    //     return Constants.MSG_SUCSESS;
    // }

    protected void ElementsShow(ref Dictionary<string, Element> diEl)
    {
        foreach (KeyValuePair<string, Element> pair in diEl)
        {
            pair.Value.Show();
        }
    }

    abstract public void Show();
    // {
    //     // 開いている場合は処理しない
    //     if (isOpening) return Constants.MSG_FAILED;
    //     isOpening = true;

    //     Debug.Log("Window Show [" + name + "]");

    //     // キャンバスとパネルを表示
    //     ShowCanvases(true);
    //     ShowPanels(true);

    //     // 各要素を表示
    //     ElementsShow(ref diImageEl);
    //     ElementsShow(ref diTextEl);

    //     return Constants.MSG_SUCSESS;
    // }

    protected void ElementsClose(ref Dictionary<string, Element> diEl)
    {
        foreach (KeyValuePair<string, Element> pair in diEl)
        {
            pair.Value.Close();
        }
    }

    abstract public void Close();
    // {
    //     // 開いていない場合は処理しない
    //     if (!isOpening) return Constants.MSG_FAILED;
    //     isOpening = false;

    //     Debug.Log("Window Close [" + name + "]");

    //     // キャンバスとパネルを非表示
    //     ShowCanvases(false);
    //     ShowPanels(false);

    //     // 各要素を非表示
    //     ElementsClose(ref diImageEl);
    //     ElementsClose(ref diTextEl);

    //     return Constants.MSG_SUCSESS;
    // }

    protected void ElementsExecute(ref Dictionary<string, Element> diEl)
    {
        foreach (KeyValuePair<string, Element> pair in diEl)
        {
            pair.Value.Execute();
        }
    }

    abstract public void Execute();
    // {
    //     // 開いていない場合は処理しない
    //     if (!isOpening) return;

    //     // 各要素を実行
    //     ElementsExecute(ref diImageEl);
    //     ElementsExecute(ref diTextEl);
    // }

    protected void ElementsMove(ref Dictionary<string, Element> diEl, ref Vector2 vec)
    {
        foreach (KeyValuePair<string, Element> pair in diEl)
        {
            pair.Value.Move(ref vec);
        }
    }

    abstract public void Move(ref Vector2 moveVec);
    // {
    //     if (!isOpening) return;

    //     // 各要素を移動
    //     ElementsMove(ref diImageEl, ref moveVec);
    //     ElementsMove(ref diTextEl, ref moveVec);
    // }
}
