using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class McControls : MonoBehaviour
{
    [SerializeField] private SelectBarParts senSb;
    private static int sen = 0;
    private static Vector2 senSbPos;

    private KeyCode attackKey = KeyCode.Mouse0;
    private KeyCode dropKey = KeyCode.Q;

    public void ControlBackground()
    {
        Param.popUpWindowDone = true;
    }

    public void SenEdit()
    {
        sen = (int)senSb.Val;
        senSbPos = senSb.SelectorPos;

        Debug.Log("Sensitivity: " + sen);

        Param.popUpWindowDone = true;
    }

    public void BindEdit(string name)
    {
        Debug.Log("BindEdit [" + name + "]");
    }

    public void BindReset(string name)
    {
        Debug.Log("BindReset [" + name + "]");
    }

    public void BindResetAll()
    {
        Debug.Log("BindResetAll");
        Param.popUpWindowDone = true;
    }
    
}
