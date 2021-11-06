using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableSub : MonoBehaviour
{
    Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void PickUp(Transform parent)
    {
        // сделать его дочерним по отношеню к парент
        rb.isKinematic = true;
        transform.SetParent(parent);
        transform.position = parent.position;
        transform.rotation = parent.rotation;
    }

    public void Drop()
    {
        rb.isKinematic = false;
        transform.parent = null;
    }
}
