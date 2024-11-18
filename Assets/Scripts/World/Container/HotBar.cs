using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotBar : Container
{
    private int selectingSlot = 1;
    public int SelectingSlot{ get { return selectingSlot; } }

    [SerializeField] private GameObject selectFrame;
    [SerializeField] private float selectFrameMoveVal = 98.0f;

    public void SelectSlot(int slot)
    {
        if (!(slot >= 1 && slot <= 9)) return;

        selectFrame.transform.position = new Vector3
        (
            selectFrame.transform.position.x + (slot - selectingSlot) * selectFrameMoveVal,
            selectFrame.transform.position.y,
            selectFrame.transform.position.z
        );

        selectingSlot = slot;
    }
}
