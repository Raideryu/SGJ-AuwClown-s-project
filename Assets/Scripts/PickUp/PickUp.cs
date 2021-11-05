using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    ChangableCursor cursor;
    // Start is called before the first frame update
    void Start()
    {
        cursor = FindObjectOfType<ChangableCursor>();
    }
}
