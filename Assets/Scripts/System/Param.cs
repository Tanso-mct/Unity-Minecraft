using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Param : MonoBehaviour
{
    [HideInInspector] public static int msg = Constants.MSG_NULL;
    [HideInInspector] public static float floPar = 0;
    [HideInInspector] public static string strPar = "";

    [HideInInspector] public static bool popUpWindowDone = false;

    [HideInInspector] public static bool createWorld = true;
    [HideInInspector] public static bool loadWorld = false;

    [HideInInspector] public static int worldInfoId = -1;

    public static void Init()
    {
        msg = Constants.MSG_NULL;
        floPar = 0;
        strPar = "";
        popUpWindowDone = false;
    }

    public static void InitWorldParam()
    {
        createWorld = false;
        loadWorld = false;
        worldInfoId = -1;
    }
}
