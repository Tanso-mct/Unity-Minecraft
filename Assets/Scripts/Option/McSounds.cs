using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class McSounds : MonoBehaviour
{
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

    public void SetMaster()
    {
        masterVal = (int)masterSb.Val;
        masterSbPos = masterSb.SelectorPos;

        Debug.Log("master: " + masterVal);
    }

    public void SetMusic()
    {
        musicVal = (int)musicSb.Val;
        musicSbPos = musicSb.SelectorPos;

        Debug.Log("music: " + musicVal);
    }

    public void SetBlocks()
    {
        blocksVal = (int)blocksSb.Val;
        blocksSbPos = blocksSb.SelectorPos;

        Debug.Log("blocks: " + blocksVal);
    }

    public void SetUi()
    {
        uiVal = (int)uiSb.Val;
        uiSbPos = uiSb.SelectorPos;

        Debug.Log("ui: " + uiVal);
    }

    public void SetPlayers()
    {
        playersVal = (int)playersSb.Val;
        playersSbPos = playersSb.SelectorPos;

        Debug.Log("players: " + playersVal);
    }

    public void SetHostile()
    {
        hostileVal = (int)hostileSb.Val;
        hostileSbPos = hostileSb.SelectorPos;

        Debug.Log("hostile: " + hostileVal);
    }

    public void SetFriendly()
    {
        friendlyVal = (int)friendlySb.Val;
        friendlySbPos = friendlySb.SelectorPos;

        Debug.Log("friendly: " + friendlyVal);
    }
    
}
