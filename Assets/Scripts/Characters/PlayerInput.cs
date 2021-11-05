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
            RaycastHit hit;
            if(Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                // добавить возможность атаки и подбора предметов
                character.Move(hit.point);
            }
        }
    }
}
