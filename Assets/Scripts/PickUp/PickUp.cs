using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    ChangeCursor cursor;
    // Start is called before the first frame update
    void Start()
    {
        cursor = FindObjectOfType<ChangeCursor>();
    }

    private void OnMouseEnter()
    {
        if (cursor)
        {
            cursor.SetCursor(tag);
        }
        else
        {
            Debug.LogError("отсутствует на сцене курсор");
        }
    }

    private void OnMouseExit()
    {
        if (cursor)
        {
            cursor.ResetCursor();
        }
        else
        {
            Debug.LogError("отсутствует на сцене курсор");
        }
    }
}
