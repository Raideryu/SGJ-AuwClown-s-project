using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class TPCMausePlayerController : MonoBehaviour
{
    private Camera mainCamera;
    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            if(Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                agent.SetDestination(hit.point);
            }
        }
    }
}
