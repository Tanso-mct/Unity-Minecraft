using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Container : MonoBehaviour
{
    private List<Vaxel> items = new List<Vaxel>();
    [SerializeField] private List<ContainerSlot> slots;

    private int width = 9;
    [SerializeField] private int height = 4;

    private int blockMaxNum = 64;

    public void Init()
    {
        items = new List<Vaxel>();
    }
}
