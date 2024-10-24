using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour
{
    // Set FPS. Make sure that fps is this value.
    public const int SPECIFIED_FPS = 60;

    // Screen resolution
    public const int SCREEN_WIDTH = 1920;
    public const int SCREEN_HEIGHT = 1080;

    // Message processing parameters
    public const int MSG_NULL = 0;
    public const int MSG_SUCCESS = 1;
    public const int MSG_FAILED = 2;
    public const int MSG_ERROR = 3;
    public const int MSG_WARNING = 4;
    public const int MSG_CHANGE_SCENE = 5;
    public const int MSG_QUIT_GAME = 6;

    // Scene name
    public const string SCENE_MENU = "Menu";
    public const string SCENE_PLAY = "Play";

    // Element type
    public const string TYPE_IMAGE = "Image";
    public const string TYPE_INPUT_BOX = "InputBox";
    public const string TYPE_BUTTON = "Button";
    public const string TYPE_TEXT = "Text";
    public const string TYPE_SELECT_BAR = "SelectBar";

    // Control name
    public const string CONTROL_ATTACK = "Attack";
    public const string CONTROL_DROP_ITEM = "DropItem";
    public const string CONTROL_USE = "Use";

    public const string CONTROL_HS1 = "HS1";
    public const string CONTROL_HS2 = "HS2";
    public const string CONTROL_HS3 = "HS3";
    public const string CONTROL_HS4 = "HS4";
    public const string CONTROL_HS5 = "HS5";
    public const string CONTROL_HS6 = "HS6";
    public const string CONTROL_HS7 = "HS7";
    public const string CONTROL_HS8 = "HS8";
    public const string CONTROL_HS9 = "HS9";

    public const string CONTROL_INVENTORY = "Inventory";
    public const string CONTROL_PERSPECTIVE = "Perspective";

    public const string CONTROL_JUMP = "Jump";
    public const string CONTROL_SPRINT = "Sprint";
    public const string CONTROL_LEFT = "Left";
    public const string CONTROL_RIGHT = "Right";
    public const string CONTROL_BACK = "Back";
    public const string CONTROL_FORWARD = "Forward";


    // Window name
    public const string WND_MENU = "WindowMenu";

    // Vaxel state
    public const string VAXEL_STATE_NULL = "Null";
    public const string VAXEL_STATE_ITEM = "Item";
    public const string VAXEL_STATE_BLOCK = "Block";
    public const string VAXEL_STATE_ENTITY = "Entity";



}
