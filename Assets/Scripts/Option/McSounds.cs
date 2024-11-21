 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class McSounds : MonoBehaviour
{
    private static bool hasSaveData = false;

    [SerializeField] SelectBarParts masterSb;
    [SerializeField] SelectBarParts musicSb;
    [SerializeField] SelectBarParts blocksSb;
    [SerializeField] SelectBarParts uiSb;
    [SerializeField] SelectBarParts playersSb;

    private static int masterVolume = 50;
    private static Vector2 masterSbPos;

    private static int musicVolume = 50;
    private static Vector2 musicSbPos;

    private static int blocksVolume = 50;
    private static Vector2 blocksSbPos;

    private static int uiVolume = 50;
    private static Vector2 uiSbPos;

    private static int playersVolume = 50;
    private static Vector2 playersSbPos;

    [SerializeField] private AudioSource sourceMusic;
    [SerializeField] private AudioSource sourceBlock;
    [SerializeField] private AudioSource sourceUI;
    [SerializeField] private AudioSource sourcePlayer;

    [SerializeField] private List<AudioClip> musicClips;
    [SerializeField] private List<AudioClip> blocksClips;
    [SerializeField] private List<AudioClip> uiClips;
    [SerializeField] private List<AudioClip> playersClips;

    private Dictionary<string, int> musicClipsIndex;
    private Dictionary<string, int> blocksClipsIndex;
    private Dictionary<string, int> uiClipsIndex;
    private Dictionary<string, int> playersClipsIndex;

    public void Init()
    {
        if (hasSaveData)
        {
            // Load data
        }

        masterSb.Init(masterVolume);
        SetMaster();

        musicSb.Init(musicVolume);
        SetMusic();

        blocksSb.Init(blocksVolume);
        SetBlocks();

        uiSb.Init(uiVolume);
        SetUi();

        playersSb.Init(playersVolume);
        SetPlayers();
    }

    public void Save()
    {
        // Save data
    }

    public void SetMaster()
    {
        masterVolume = (int)masterSb.Val;
        masterSbPos = masterSb.SelectorPos;

        masterSb.EditTxt("Master Volume: " + masterVolume + "%");
    }

    public void SetMusic()
    {
        musicVolume = (int)musicSb.Val;
        musicSbPos = musicSb.SelectorPos;

        musicSb.EditTxt("Music: " + musicVolume + "%");
    }

    public void SetBlocks()
    {
        blocksVolume = (int)blocksSb.Val;
        blocksSbPos = blocksSb.SelectorPos;

        blocksSb.EditTxt("Blocks: " + blocksVolume + "%");
    }

    public void SetUi()
    {
        uiVolume = (int)uiSb.Val;
        uiSbPos = uiSb.SelectorPos;

        uiSb.EditTxt("UI: " + uiVolume + "%");
    }

    public void SetPlayers()
    {
        playersVolume = (int)playersSb.Val;
        playersSbPos = playersSb.SelectorPos;

        playersSb.EditTxt("Players: " + playersVolume + "%");
    }
}
