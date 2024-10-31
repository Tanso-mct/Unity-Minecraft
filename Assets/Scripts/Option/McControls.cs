using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class McControls : MonoBehaviour
{
    private static bool hasSaveData = false;

    [SerializeField] private SelectBarParts senSb;
    private static int sen = 2;
    private static Vector2 senSbPos;

    static private KeyCode attackKey = KeyCode.Mouse0;
    static private KeyCode dropKey = KeyCode.Q;
    static private KeyCode useKey = KeyCode.Mouse1;

    static private KeyCode hotBar1Key = KeyCode.Alpha1;
    static private KeyCode hotBar2Key = KeyCode.Alpha2;
    static private KeyCode hotBar3Key = KeyCode.Alpha3;
    static private KeyCode hotBar4Key = KeyCode.Alpha4;
    static private KeyCode hotBar5Key = KeyCode.Alpha5;
    static private KeyCode hotBar6Key = KeyCode.Alpha6;
    static private KeyCode hotBar7Key = KeyCode.Alpha7;
    static private KeyCode hotBar8Key = KeyCode.Alpha8;
    static private KeyCode hotBar9Key = KeyCode.Alpha9;

    static private KeyCode inventoryKey = KeyCode.E;

    static private KeyCode jumpKey = KeyCode.Space;
    static private KeyCode sprintKey = KeyCode.LeftShift;
    static private KeyCode leftKey = KeyCode.A;
    static private KeyCode rightKey = KeyCode.D;
    static private KeyCode backKey = KeyCode.S;
    static private KeyCode forwardKey = KeyCode.W;


    public void Init()
    {
        if (hasSaveData)
        {
            // Load data
        }
    }

    public void Save()
    {
        // Save data
    }

    public void ControlBackground()
    {
        Param.popUpWindowDone = true;
    }

    public void SenEdit()
    {
        sen = (int)senSb.Val;
        senSbPos = senSb.SelectorPos;

        senSb.EditTxt("Sensitivity: " + sen + "%");

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

    static public void CursorLock(bool symbol)
    {
        if (symbol)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public static Vector2 GetMouseAxis()
    {
        Vector2 rtVec = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        rtVec.x *= sen;
        rtVec.y *= sen;
        return rtVec;
    }

    static public bool IsKey(string control)
    {
        switch (control)
        {
            case Constants.CONTROL_ATTACK:
                return Input.GetKey(attackKey);

            case Constants.CONTROL_DROP_ITEM:
                return Input.GetKey(dropKey);

            case Constants.CONTROL_USE:
                return Input.GetKey(useKey);

            case Constants.CONTROL_HS1:
                return Input.GetKey(hotBar1Key);

            case Constants.CONTROL_HS2:
                return Input.GetKey(hotBar2Key);

            case Constants.CONTROL_HS3:
                return Input.GetKey(hotBar3Key);

            case Constants.CONTROL_HS4:
                return Input.GetKey(hotBar4Key);

            case Constants.CONTROL_HS5:
                return Input.GetKey(hotBar5Key);

            case Constants.CONTROL_HS6:
                return Input.GetKey(hotBar6Key);

            case Constants.CONTROL_HS7:
                return Input.GetKey(hotBar7Key);

            case Constants.CONTROL_HS8:
                return Input.GetKey(hotBar8Key);

            case Constants.CONTROL_HS9:
                return Input.GetKey(hotBar9Key);

            case Constants.CONTROL_INVENTORY:
                return Input.GetKey(inventoryKey);

            case Constants.CONTROL_JUMP:
                return Input.GetKey(jumpKey);

            case Constants.CONTROL_SPRINT:
                return Input.GetKey(sprintKey);

            case Constants.CONTROL_LEFT:
                return Input.GetKey(leftKey);

            case Constants.CONTROL_RIGHT:
                return Input.GetKey(rightKey);

            case Constants.CONTROL_BACK:
                return Input.GetKey(backKey);

            case Constants.CONTROL_FOR:
                return Input.GetKey(forwardKey);

            default: return false;
        }
    }

    static public bool IsKeyDown(string control)
    {
        switch (control)
        {
            case Constants.CONTROL_ATTACK:
                return Input.GetKeyDown(attackKey);

            case Constants.CONTROL_DROP_ITEM:
                return Input.GetKeyDown(dropKey);

            case Constants.CONTROL_USE:
                return Input.GetKeyDown(useKey);

            case Constants.CONTROL_HS1:
                return Input.GetKeyDown(hotBar1Key);

            case Constants.CONTROL_HS2:
                return Input.GetKeyDown(hotBar2Key);

            case Constants.CONTROL_HS3:
                return Input.GetKeyDown(hotBar3Key);

            case Constants.CONTROL_HS4:
                return Input.GetKeyDown(hotBar4Key);

            case Constants.CONTROL_HS5:
                return Input.GetKeyDown(hotBar5Key);

            case Constants.CONTROL_HS6:
                return Input.GetKeyDown(hotBar6Key);

            case Constants.CONTROL_HS7:
                return Input.GetKeyDown(hotBar7Key);

            case Constants.CONTROL_HS8:
                return Input.GetKeyDown(hotBar8Key);

            case Constants.CONTROL_HS9:
                return Input.GetKeyDown(hotBar9Key);

            case Constants.CONTROL_INVENTORY:
                return Input.GetKeyDown(inventoryKey);

            case Constants.CONTROL_JUMP:
                return Input.GetKeyDown(jumpKey);

            case Constants.CONTROL_SPRINT:
                return Input.GetKeyDown(sprintKey);

            case Constants.CONTROL_LEFT:
                return Input.GetKeyDown(leftKey);

            case Constants.CONTROL_RIGHT:
                return Input.GetKeyDown(rightKey);

            case Constants.CONTROL_BACK:
                return Input.GetKeyDown(backKey);

            case Constants.CONTROL_FOR:
                return Input.GetKeyDown(forwardKey);

            default: return false;
        }
    }

    static public bool IsKeyUp(string control)
    {
        switch (control)
        {
            case Constants.CONTROL_ATTACK:
                return Input.GetKeyUp(attackKey);

            case Constants.CONTROL_DROP_ITEM:
                return Input.GetKeyUp(dropKey);

            case Constants.CONTROL_USE:
                return Input.GetKeyUp(useKey);

            case Constants.CONTROL_HS1:
                return Input.GetKeyUp(hotBar1Key);

            case Constants.CONTROL_HS2:
                return Input.GetKeyUp(hotBar2Key);

            case Constants.CONTROL_HS3:
                return Input.GetKeyUp(hotBar3Key);

            case Constants.CONTROL_HS4:
                return Input.GetKeyUp(hotBar4Key);

            case Constants.CONTROL_HS5:
                return Input.GetKeyUp(hotBar5Key);

            case Constants.CONTROL_HS6:
                return Input.GetKeyUp(hotBar6Key);

            case Constants.CONTROL_HS7:
                return Input.GetKeyUp(hotBar7Key);

            case Constants.CONTROL_HS8:
                return Input.GetKeyUp(hotBar8Key);

            case Constants.CONTROL_HS9:
                return Input.GetKeyUp(hotBar9Key);

            case Constants.CONTROL_INVENTORY:
                return Input.GetKeyUp(inventoryKey);

            case Constants.CONTROL_JUMP:
                return Input.GetKeyUp(jumpKey);

            case Constants.CONTROL_SPRINT:
                return Input.GetKeyUp(sprintKey);

            case Constants.CONTROL_LEFT:
                return Input.GetKeyUp(leftKey);

            case Constants.CONTROL_RIGHT:
                return Input.GetKeyUp(rightKey);

            case Constants.CONTROL_BACK:
                return Input.GetKeyUp(backKey);

            case Constants.CONTROL_FOR:
                return Input.GetKeyUp(forwardKey);

            default: return false;
        }
    }
    
}
