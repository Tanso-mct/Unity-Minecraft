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
    [SerializeField] SelectBarParts hostileSb;
    [SerializeField] SelectBarParts friendlySb;

    private static int masterVal = 50;
    private static Vector2 masterSbPos;

    private static int musicVal = 50;
    private static Vector2 musicSbPos;

    private static int blocksVal = 50;
    private static Vector2 blocksSbPos;

    private static int uiVal = 50;
    private static Vector2 uiSbPos;

    private static int playersVal = 50;
    private static Vector2 playersSbPos;

    private static int hostileVal = 50;
    private static Vector2 hostileSbPos;

    private static int friendlyVal = 50;
    private static Vector2 friendlySbPos;

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

    public void SetMaster()
    {
        masterVal = (int)masterSb.Val;
        masterSbPos = masterSb.SelectorPos;

        masterSb.EditTxt("Master Volume: " + masterVal + "%");
    }

    public void SetMusic()
    {
        musicVal = (int)musicSb.Val;
        musicSbPos = musicSb.SelectorPos;

        musicSb.EditTxt("Music: " + musicVal + "%");
    }

    public void SetBlocks()
    {
        blocksVal = (int)blocksSb.Val;
        blocksSbPos = blocksSb.SelectorPos;

        blocksSb.EditTxt("Blocks: " + blocksVal + "%");
    }

    public void SetUi()
    {
        uiVal = (int)uiSb.Val;
        uiSbPos = uiSb.SelectorPos;

        uiSb.EditTxt("UI: " + uiVal + "%");
    }

    public void SetPlayers()
    {
        playersVal = (int)playersSb.Val;
        playersSbPos = playersSb.SelectorPos;

        playersSb.EditTxt("Players: " + playersVal + "%");
    }

    public void SetHostile()
    {
        hostileVal = (int)hostileSb.Val;
        hostileSbPos = hostileSb.SelectorPos;

        hostileSb.EditTxt("Hostile Creatures: " + hostileVal + "%");
    }

    public void SetFriendly()
    {
        friendlyVal = (int)friendlySb.Val;
        friendlySbPos = friendlySb.SelectorPos;

        friendlySb.EditTxt("Friendly Creatures: " + friendlyVal + "%");
    }
    
}
