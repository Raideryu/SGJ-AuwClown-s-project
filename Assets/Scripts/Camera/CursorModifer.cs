using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorModifer : MonoBehaviour
{
    [SerializeField]
    CursorModifireType modifireType = CursorModifireType.AttackCursor;
    private ChangableCursor cursor;
    private void Start()
    {
        cursor = FindObjectOfType<ChangableCursor>();
    }
    private void OnMouseEnter()
    {
        if (cursor)
        {
            cursor.SetCursor(modifireType);
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
