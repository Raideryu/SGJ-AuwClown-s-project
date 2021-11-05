using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangableCursor : MonoBehaviour
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
    public void SetCursor(CursorModifireType tag)
    {
        switch (tag)
        {
            case CursorModifireType.DefaultCursor:
                Cursor.SetCursor(cursorDefault, Vector2.zero, CursorMode.ForceSoftware);
                break;
            case CursorModifireType.AttackCursor:
                Cursor.SetCursor(cursorAttack, Vector2.zero, CursorMode.ForceSoftware);
                break;
            case CursorModifireType.PickUpCursor:
                Cursor.SetCursor(cursorPickUp, Vector2.zero, CursorMode.ForceSoftware);
                break;
        }
    }

    public void ResetCursor()
    {
        Cursor.SetCursor(cursorDefault, Vector2.zero, CursorMode.ForceSoftware);
    }
}
public enum CursorModifireType
{
    DefaultCursor, AttackCursor, PickUpCursor
}
