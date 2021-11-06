using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableSub : MonoBehaviour
{
    //[SerializeField] Transform handPosition;
    private void Start()
    {
      
    }
    public void PickUp(Transform parent)
    {
        // сделать его дочерним по отношеню к парент
        transform.SetParent(parent);
        transform.position = parent.position;
        transform.rotation = parent.rotation;
    }
}
