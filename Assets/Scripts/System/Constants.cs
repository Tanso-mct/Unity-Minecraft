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

    // Window name
    public static readonly string WND_MENU = "WindowMenu";

    // Vaxel state
    public static readonly string VAXEL_STATE_NULL = "Null";
    public static readonly string VAXEL_STATE_ITEM = "Item";
    public static readonly string VAXEL_STATE_BLOCK = "Block";
    public static readonly string VAXEL_STATE_ENTITY = "Entity";


}
