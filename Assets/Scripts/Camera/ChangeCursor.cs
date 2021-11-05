using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCursor : MonoBehaviour
{
    [SerializeField]
    private Texture2D cursorDefault;
    [SerializeField]
    private Texture2D cursorAttack;
    [SerializeField]
    private Texture2D cursorPickUp;

    void Start()
    {
        Cursor.SetCursor(cursorDefault, Vector2.zero, CursorMode.ForceSoftware);
    }
    public void SetCursor(string tag)
    {
        if(tag == "Enemy")
        {
            Cursor.SetCursor(cursorAttack, Vector2.zero, CursorMode.ForceSoftware);
        }
        else if(tag == "PickUp") 
        {
            Cursor.SetCursor(cursorPickUp, Vector2.zero, CursorMode.ForceSoftware);
        }
    }

    public void ResetCursor()
    {
        Cursor.SetCursor(cursorDefault, Vector2.zero, CursorMode.ForceSoftware);
    }
}
