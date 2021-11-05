using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    ChangeCursor cursor;
    // Start is called before the first frame update
    void Start()
    {
        cursor = FindObjectOfType<ChangeCursor>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        if (cursor)
        {
            cursor.SetCursor(tag);
        }
        else
        {
            Debug.LogError("����������� �� ����� ������");
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
            Debug.LogError("����������� �� ����� ������");
        }
    }
}
