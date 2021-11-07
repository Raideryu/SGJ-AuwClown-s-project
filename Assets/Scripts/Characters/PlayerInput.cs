using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System;

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

            List<RaycastHit> hitsList = hits.ToList();


            if (hitsList.Exists(e => e.collider.gameObject.tag == "UI"))
            {
                return;
            }
            if (hitsList.Exists(e => e.collider.gameObject.tag == "Enemy"))
            {
                RaycastHit hit = hitsList.Find(e => e.collider.gameObject.tag == "Enemy");
                character.MoveToWithAction(hit.point, hit.collider.gameObject);
                return;
            }
            if (hitsList.Exists(e => e.collider.gameObject.tag == "PickUp"))
            {
                RaycastHit hit = hitsList.Find(e => e.collider.gameObject.tag == "PickUp");
                character.MoveToWithAction(hit.point, hit.collider.gameObject);
                return;
            }
            if (hitsList.Exists(e => e.collider.gameObject.tag == "Ground"))
            {
                RaycastHit hit = hitsList.Find(e => e.collider.gameObject.tag == "Ground");
                character.MoveToWithAction(hit.point, hit.collider.gameObject);
                return;
            }
           

            
        }
    }
}

