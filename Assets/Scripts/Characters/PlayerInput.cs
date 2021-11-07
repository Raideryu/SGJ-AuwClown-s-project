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
    //public GameObject selectCircle = null;

    void Start()
    {
        mainCamera = Camera.main;
        //selectCircle.SetActive(false);
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
        /// � ���� �� ����� ���� 0 ��� ������ ����� ���� �� ��������
        /// ���� ����� ��� �� basecharacter �������� "�������" ���������
        if (Input.GetKeyDown(KeyCode.E))
        {
            isTpAction = true;
        }
        if (isTpAction)
        {
            if (Input.GetMouseButton(0)) 
            {
                RaycastHit p_hit;
                if (rayMousePosition("Ground", out p_hit))
                {
                    character.TeleportToTarget(p_hit.collider.transform.position);
                    isTpAction = false;
                    //selectCircle.transform.position = new Vector3(p_hit.point.x, p_hit.point.y + 0.5f, p_hit.point.z);
                   // selectCircle.SetActive(true);
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isTpAction = false;
                //selectCircle.SetActive(false);
            }
        }
        else
            //selectCircle.SetActive(false);
        

        /// ������� �������� �������� �� ������� ���������������� ������ �� inventar
        /// � ������� �� basecharacter.InputSpel(spel type) 
        /// ����� ������ �� ������
        /// 
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (inventar.currentWeapon)
            {
                character.InputSpel(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (inventar.currentSecondWeapon)
            {
                character.InputSpel(false);
            }
        }
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
    private bool rayMousePosition(string _tag, out RaycastHit p_hit)
    {
        Ray p_ray;
        p_ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(p_ray, out p_hit) && p_hit.collider.CompareTag(_tag))
        {
            return true;
        }
        return false;
    }
}

