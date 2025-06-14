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

    // Key bind
    public const  KeyCode KEY_ATTACK = KeyCode.Mouse0;
    public const  KeyCode KEY_DROP = KeyCode.Q;
    public const  KeyCode KEY_USE = KeyCode.Mouse1;

    public const  KeyCode KEY_HOT_BAR_1 = KeyCode.Alpha1;
    public const  KeyCode KEY_HOT_BAR_2 = KeyCode.Alpha2;
    public const  KeyCode KEY_HOT_BAR_3 = KeyCode.Alpha3;
    public const  KeyCode KEY_HOT_BAR_4 = KeyCode.Alpha4;
    public const  KeyCode KEY_HOT_BAR_5 = KeyCode.Alpha5;
    public const  KeyCode KEY_HOT_BAR_6 = KeyCode.Alpha6;
    public const  KeyCode KEY_HOT_BAR_7 = KeyCode.Alpha7;
    public const  KeyCode KEY_HOT_BAR_8 = KeyCode.Alpha8;
    public const  KeyCode KEY_HOT_BAR_9 = KeyCode.Alpha9;

    public const  KeyCode KEY_INVENTORY = KeyCode.E;

    public const  KeyCode KEY_JUMP = KeyCode.Space;
    public const  KeyCode KEY_SPRINT = KeyCode.LeftShift;
    public const  KeyCode KEY_LEFT = KeyCode.A;
    public const  KeyCode KEY_RIGHT = KeyCode.D;
    public const  KeyCode KEY_BACK = KeyCode.S;
    public const  KeyCode KEY_FORWARD = KeyCode.W;

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

    public const string CONTROL_JUMP = "Jump";
    public const string CONTROL_SPRINT = "Sprint";
    public const string CONTROL_LEFT = "Left";
    public const string CONTROL_RIGHT = "Right";
    public const string CONTROL_BACK = "Back";
    public const string CONTROL_FOR = "Forward";

    // Game mode
    public const string GAME_MODE_CREATIVE = "Creative";
    public const string GAME_MODE_SURVIVAL = "Survival";

    // Window name
    public const string WND_MENU = "WindowMenu";

    // Vaxel state
    public const int VAXEL_STATE_NULL = 0;
    public const int VAXEL_STATE_ITEM = 1;
    public const int VAXEL_STATE_BLOCK = 2;
    public const int VAXEL_STATE_ENTITY = 3;

    // Vaxel type
    public const int BLOCK_TYPE_CANT_SET = -1;
    public enum BLOCK_TYPE
    {
        AIR,
        BEDROCK,
        DIRT,
        GRASS_TOP, GRASS_SIDE, GRASS_BOTTOM,
        STONE, COBBLESTONE, STONE_ANDESITE, STONE_DIORITE, STONE_GRANITE,
        COAL_ORE,
        IRON_ORE,
        GOLD_ORE,
        DIAMOND_ORE,
        EMERALD_ORE,
        LAPIS_ORE
    }

    public const int TARGET_BLOCK_SELECT = 0;
    public const int TARGET_BLOCK_SET = 1;

    // Block object type    
    public const string BLOCK_OBJ_FRONT = "Front";
    public const string BLOCK_OBJ_BACK = "Back";
    public const string BLOCK_OBJ_LEFT = "Left";
    public const string BLOCK_OBJ_RIGHT = "Right";
    public const string BLOCK_OBJ_TOP = "Top";
    public const string BLOCK_OBJ_BOTTOM = "Bottom";

    public const int CHUCK_SIZE = 16;
    

    // Texture face
    public const int TEXTURE_FACE_FULL = 0;
    public const int TEXTURE_FACE_TOP = 0;
    public const int TEXTURE_FACE_SIDE = 2;
    public const int TEXTURE_FACE_BOTTOM = 1;
    public const int TEXTURE_FACE_FRONT = 2;
    public const int TEXTURE_FACE_BACK = 3;
    public const int TEXTURE_FACE_LEFT = 4;
    public const int TEXTURE_FACE_RIGHT = 5;

    // Texture atlas path
    public const string TEXTURE_ATLAS_BLOCK = "Textures/Atlas/blocks";

    // Texture path
    public const string TEXTURE_BEDROCK = "Textures/blocks/bedrock";
    public const string TEXTURE_DIRT = "Textures/blocks/dirt";
    public const string TEXTURE_GRASS_TOP = "Textures/blocks/grass_top";
    public const string TEXTURE_GRASS_SIDE = "Textures/blocks/grass_side";
    public const string TEXTURE_GRASS_BOTTOM = "Textures/blocks/dirt";
    public const string TEXTURE_STONE = "Textures/blocks/stone";
    public const string TEXTURE_COBBLESTONE = "Textures/blocks/cobblestone";
    public const string TEXTURE_STONE_ANDESITE = "Textures/blocks/stone_andesite";
    public const string TEXTURE_STONE_DIORITE = "Textures/blocks/stone_diorite";
    public const string TEXTURE_STONE_GRANITE = "Textures/blocks/stone_granite";
    public const string TEXTURE_COAL_ORE = "Textures/blocks/coal_ore";
    public const string TEXTURE_IRON_ORE = "Textures/blocks/iron_ore";
    public const string TEXTURE_GOLD_ORE = "Textures/blocks/gold_ore";
    public const string TEXTURE_DIAMOND_ORE = "Textures/blocks/diamond_ore";
    public const string TEXTURE_EMERALD_ORE = "Textures/blocks/emerald_ore";
    public const string TEXTURE_REDSTONE_ORE = "Textures/blocks/redstone_ore";
    public const string TEXTURE_LAPIS_ORE = "Textures/blocks/lapis_ore";

    // World Prefab path
    public const string PREFAB_VAXEL = "World/Vaxel";
    public const string PREFAB_BLOCK = "World/Block";

    // World size
    public const int WORLD_SIZE = 513;
    public const int WORLD_HALF_SIZE = 256;
    public const int WORLD_HEIGHT = 320;


    // World Info
    public const string WORLD_MODE_CREATIVE = "Creative";
    public const string WORLD_MODE_SURVIVAL = "Survival";
    public const string WORLD_TYPE_NORMAL = "Normal";
    public const string WORLD_TYPE_FLAT = "Flat";


    // Animation
    public const string ANIM_TYPE = "AnimType";
    public const int ANIM_IDLE = 0;
    public const int ANIM_PLAYER_BREATH = 1;
    public const int ANIM_PLAYER_WALK = 2;
    public const int ANIM_PLAYER_RUN = 3;
    public const int ANIM_PLAYER_USE = 4;

    // Unity Tags
    public const string TAG_PLAYER = "Player";
    public const string TAG_BLOCK_TOP = "Block_Top";

    // ソースメッシュの頂点数、UV数、頂点インデックス数のうち最大のもの
    public static int SOURCE_MESH_VS_MAX = 0;
    public static int SOURCE_MESH_TRIS_MAX = 0;

    public static void SetShaderConstants(ref ComputeShader shader)
    {
        shader.SetInt("CHUCK_SIZE", CHUCK_SIZE);
        shader.SetInt("WORLD_SIZE", WORLD_SIZE);
        shader.SetInt("WORLD_HALF_SIZE", WORLD_HALF_SIZE);
        shader.SetInt("WORLD_HEIGHT", WORLD_HEIGHT);
    }

}
