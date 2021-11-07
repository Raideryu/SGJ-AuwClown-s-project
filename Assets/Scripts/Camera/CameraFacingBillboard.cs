using UnityEngine;
using System.Collections;

public class CameraFacingBillboard : MonoBehaviour
{
    [HideInInspector]public Camera m_Camera;

    private void Start()
    {
        m_Camera = Camera.main;
        GetComponent<Canvas>().worldCamera = m_Camera;
    }

    //Orient the camera after all movement is completed this frame to avoid jittering
    void LateUpdate()
    {
        Vector3 a = m_Camera.transform.position ;
        a.y = transform.position.y;
        transform.LookAt(a);
    }
}

