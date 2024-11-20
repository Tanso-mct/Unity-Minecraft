using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EntityItem : MonoBehaviour
{
    public bool isThrow = false;
    private Vector3 throwDirection;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private BoxCollider bc;

    void Update()
    {
        if (isThrow && !IsMoving())
        {
            isThrow = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    public void ThrowIt(Vector3 direction)
    {
        isThrow = true;
        throwDirection = direction;

        // PhysicMaterial??????
        PhysicMaterial material = new PhysicMaterial();
        material.dynamicFriction = 1.0f;
        material.staticFriction = 1.0f;
        material.frictionCombine = PhysicMaterialCombine.Maximum;

        bc.material = material;

        rb.AddForce(throwDirection, ForceMode.Impulse);
    }

    private bool IsMoving()
    {
        return rb.velocity != Vector3.zero;
    }
}
