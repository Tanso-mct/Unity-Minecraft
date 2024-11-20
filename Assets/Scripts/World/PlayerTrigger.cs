using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Enter with " + other.gameObject.name);
    }   

    void OnTriggerStay(Collider other)
    {
        // Debug.Log("Trigger Stay with " + other.gameObject.name);
    }

    void OnTriggerExit(Collider other)
    {
        // ƒgƒŠƒK[‚©‚ço‚½‚Æ‚«‚Ìˆ—
    }
}
