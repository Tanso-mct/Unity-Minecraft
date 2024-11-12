using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent onUseEnd;

    public void UseEnd()
    {
        onUseEnd.Invoke();
    }
}
