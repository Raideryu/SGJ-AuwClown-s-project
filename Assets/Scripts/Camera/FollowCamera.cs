using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField]
    float cameraSpeed = 1;
    private float offsetX;
    private float offsetY;
    private float offsetZ;
    [SerializeField,Tooltip("может быть пустым, если есть TPCMausePlayerController на игроке")]
    GameObject player;
    
    void Start()
    {
        if (!player)
            player = FindObjectOfType<PlayerInput>().gameObject;

        offsetX = player.transform.position.x - transform.position.x;
        offsetY = player.transform.position.y - transform.position.y;
        offsetZ = player.transform.position.z - transform.position.z;
    }

    

    private void FixedUpdate()
    {
        float nextPosX = player.transform.position.x;
        float nextPosY = player.transform.position.y;
        float nextPosZ = player.transform.position.z;
        float xPos = Mathf.Lerp(transform.position.x, nextPosX - offsetX, cameraSpeed * Time.fixedDeltaTime);
        float yPos = Mathf.Lerp(transform.position.y, nextPosY - offsetY, cameraSpeed * Time.fixedDeltaTime);
        float zPos = Mathf.Lerp(transform.position.z, nextPosZ - offsetZ, cameraSpeed * Time.fixedDeltaTime);


        Vector3 nextCameraPos = new Vector3(xPos - transform.position.x, yPos - transform.position.y, zPos - transform.position.z);
        
        transform.Translate(nextCameraPos, Space.World);
    }
}
