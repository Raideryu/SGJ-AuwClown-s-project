using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(BaseCharacter))]
public class PlayerInput : MonoBehaviour
{
    private Camera mainCamera;
    private BaseCharacter character;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        character = GetComponent<BaseCharacter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit[] hits = Physics.RaycastAll(mainCamera.ScreenPointToRay(Input.mousePosition));

            foreach (RaycastHit hit in hits)
            {
                if(hit.collider.tag == "Ground")
                {
                    character.MoveToWithAction(hit.point, hit.collider.gameObject);
                    break;
                }                
            }

            // RaycastHit hit;
            //if(Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit))
            //{
            //    // второй параметр отвечает за текущее действие
            //    character.MoveToWithAction(hit.point, hit.collider.gameObject);

            //}
        }
    }
}

