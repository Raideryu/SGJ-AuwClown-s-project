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
    private CharacterInventar inventar;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        character = GetComponent<BaseCharacter>();
        inventar = GetComponent<CharacterInventar>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }
    bool isTpAction = false;
    private void Movement()
    {
        //������ �������
        // ��� ������� �� � ���������� bool isTpAction = true
        /// � ���� �� ����� ���� 0 ��� ������ ����� ���� �� ��������
        /// ���� ����� ��� �� basecharacter �������� "�������" ���������
        /// 
        /// ���� ����� ��������������� ������ �� ����� (Q,W,E), 
        /// ������� �������� �������� �� ������� ���������������� ������ �� inventar
        /// � ������� �� basecharacter.InputSpel(spel type) 
        /// ����� ������ �� ������
        //����� �������


        /// ��������� ����� ������� ����, �� �������� ���� ������ �
        if (Input.GetMouseButton(0) && !isTpAction)
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

