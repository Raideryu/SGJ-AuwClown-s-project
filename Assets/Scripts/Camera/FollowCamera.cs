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

    [SerializeField]
    Vector3 cameraOffset;
    [SerializeField,Tooltip("может быть пустым, если есть TPCMausePlayerController на игроке")]
    GameObject player;
    


    void Start()
    {
        if (!player)
        {
            if (FindObjectOfType<PlayerInput>())
                player = FindObjectOfType<PlayerInput>().gameObject;
            else
                return;
        }

        if (player)
        {
            cameraOffset = player.transform.position - transform.position;
            offsetX = player.transform.position.x - transform.position.x;
            offsetY = player.transform.position.y - transform.position.y;
            offsetZ = player.transform.position.z - transform.position.z;
        }
        
    }

    private void Update()
    {
        if (!player)
        {
            if (FindObjectOfType<PlayerInput>())
                player = FindObjectOfType<PlayerInput>().gameObject;
            else
                return;
        }

            float nextPosX = player.transform.position.x;
        float nextPosY = player.transform.position.y;
        float nextPosZ = player.transform.position.z;
        float xPos = Mathf.Lerp(transform.position.x, nextPosX - offsetX, cameraSpeed * Time.deltaTime);
        float yPos = Mathf.Lerp(transform.position.y, nextPosY - offsetY, cameraSpeed * Time.deltaTime);
        float zPos = Mathf.Lerp(transform.position.z, nextPosZ - offsetZ, cameraSpeed * Time.deltaTime);

        Vector3 newCameraPos = Vector3.Lerp(transform.position, player.transform.position - cameraOffset, cameraSpeed * Time.deltaTime) - transform.position;

        //Vector3 newV

      //  Vector3 nextCameraPos = new Vector3(xPos - transform.position.x, yPos - transform.position.y, zPos - transform.position.z);

        transform.Translate(newCameraPos, Space.World);


    }

}
