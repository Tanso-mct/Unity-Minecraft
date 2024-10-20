using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour
{
    // Set FPS. Make sure that fps is this value.
    public static readonly int SPECIFIED_FPS = 60;

    // Screen resolution
    public static readonly int SCREEN_WIDTH = 1920;
    public static readonly int SCREEN_HEIGHT = 1080;

    // Message processing parameters
    public static readonly int MSG_NULL = 0;
    public static readonly int MSG_SUCCESS = 1;
    public static readonly int MSG_FAILED = 2;
    public static readonly int MSG_ERROR = 3;
    public static readonly int MSG_WARNING = 4;
    public static readonly int MSG_CHANGE_SCENE = 5;

    // Scene name
    public static readonly string SCENE_MENU = "Menu";
    public static readonly string SCENE_PLAY = "Play";

    // Element type
    public static readonly string TYPE_IMAGE = "Image";
    public static readonly string TYPE_INPUT_BOX = "InputBox";
    public static readonly string TYPE_BUTTON = "Button";
    public static readonly string TYPE_TEXT = "Text";
    public static readonly string TYPE_SELECT_BAR = "SelectBar";

    // Control name
    public static readonly string CONTROL_ATTACK = "Attack";
    public static readonly string CONTROL_DROP_ITEM = "DropItem";
    public static readonly string CONTROL_USE = "Use";
    public static readonly string CONTROL_HS1 = "HS1";
    public static readonly string CONTROL_HS2 = "HS2";
    public static readonly string CONTROL_HS3 = "HS3";
    public static readonly string CONTROL_HS4 = "HS4";
    public static readonly string CONTROL_HS5 = "HS5";
    public static readonly string CONTROL_HS6 = "HS6";
    public static readonly string CONTROL_HS7 = "HS7";
    public static readonly string CONTROL_HS8 = "HS8";
    public static readonly string CONTROL_HS9 = "HS9";
    public static readonly string CONTROL_INVENTORY = "Inventory";
    public static readonly string CONTROL_PERSPECTIVE = "Perspective";
    public static readonly string CONTROL_JUMP = "Jump";
    public static readonly string CONTROL_SPRINT = "Sprint";
    public static readonly string CONTROL_LEFT = "Left";
    public static readonly string CONTROL_RIGHT = "Right";
    public static readonly string CONTROL_BACK = "Back";
    public static readonly string CONTROL_FORWARD = "Forward";


    // Window name
    public static readonly string WND_MENU = "WindowMenu";

    // Vaxel state
    public static readonly string VAXEL_STATE_NULL = "Null";
    public static readonly string VAXEL_STATE_ITEM = "Item";
    public static readonly string VAXEL_STATE_BLOCK = "Block";
    public static readonly string VAXEL_STATE_ENTITY = "Entity";



}
