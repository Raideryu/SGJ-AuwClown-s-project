using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomSnapPoint : MonoBehaviour
{
    public enum ConnectionType 
    {
        Floor,
        Wall,
        UpperPoint 
    }
    public ConnectionType Type;
    private void OnDrawGizmos()
    {
        switch (Type) 
        {
            case ConnectionType.Floor:
                Gizmos.color = Color.green;
                break;
            case ConnectionType.Wall:
                Gizmos.color = Color.yellow;
                break;
            case ConnectionType.UpperPoint:
                Gizmos.color = Color.red;
                break;
        }
        
        Gizmos.DrawSphere(transform.position, radius: 0.1f);
    }
}
