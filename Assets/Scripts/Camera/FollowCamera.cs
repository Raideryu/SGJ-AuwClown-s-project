using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField]
    float cameraSpeed = 1;
    [SerializeField]
    Material transparentMaterial;
    [SerializeField]
    float invizRadius = 1;
    [SerializeField]
    float invizRayOffsetY = 1;
    [SerializeField]
    Vector3 cameraOffset;
    [SerializeField, Tooltip("может быть пустым, если есть TPCMausePlayerController на игроке")]
    GameObject player;

    Camera mainCamera;
    float cameraDistance;
    void Start()
    {
        mainCamera = Camera.main;
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

        }
        cameraDistance = cameraOffset.magnitude;
    }
    float prevMousePos = -1;
    private void Update()
    {
        if (!player)
        {
            if (FindObjectOfType<PlayerInput>())
                player = FindObjectOfType<PlayerInput>().gameObject;
            else
                return;
        }
        //Vector3 newCameraPos = Vector3.Lerp(transform.position, player.transform.position - cameraOffset, cameraSpeed * Time.deltaTime) - transform.position;
        transform.Translate(player.transform.position - transform.position- cameraOffset, Space.World);
       
        float mousePos = Input.mousePosition.x;
        if (prevMousePos == -1)
            prevMousePos = mousePos;
        float deltaMouseX = (mousePos - prevMousePos) / Screen.width;
        if (Input.GetMouseButton(1))
        {
            //float mousePos = Input.mousePosition.x;
            //if (prevMousePos == -1)
            //    prevMousePos = mousePos;
            //float deltaMouseX = (mousePos - prevMousePos)/ Screen.width;
            //float mouseAngle = (mousePos - (Screen.width / 2)) / Screen.width;
            gameObject.transform.RotateAround(player.transform.position, Vector3.up, -deltaMouseX * cameraSpeed);
            cameraOffset = (player.transform.position - transform.position);
            prevMousePos = mousePos;
        }
        
        prevMousePos = mousePos;




        WallCheck();
    }

    List<GameObject> invisableMeshs = new List<GameObject>();
    List<Material> materials = new List<Material>();



    private void WallCheck()
    {
        if (!player) return;

        Vector3 playerPos = player.transform.position;
        Vector3 cameraPos = transform.position;

        RaycastHit[] hits = Physics.SphereCastAll(cameraPos, invizRadius,
            playerPos + new Vector3(0, invizRayOffsetY, 0) - cameraPos,
            Vector3.Distance(cameraPos, playerPos));


        if (hits.Length > 0)
        {
            if (invisableMeshs.Count > 0 && materials.Count == invisableMeshs.Count)
                for (int i = 0; i < invisableMeshs.Count; i++)
                {
                    if (invisableMeshs[i].TryGetComponent<Renderer>(out Renderer renderer))
                    {
                        renderer.material = materials[i];
                    }

                }
            materials.Clear();
            invisableMeshs.Clear();

            foreach (RaycastHit hit in hits)
            {
                GameObject col = hit.collider.gameObject;
                if (col.isStatic && col.tag != "Ground")
                {
                    if (col.TryGetComponent<Renderer>(out Renderer renderer))
                    {
                        materials.Add(renderer.material);
                        renderer.material = transparentMaterial;
                        invisableMeshs.Add(col);
                    }
                }
            }
        }
    }
}
