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
        //начало условий
        // при нажатии на Е записывает bool isTpAction = true
        /// и пока не нажал мышь 0 или эскейп метод ниже не работает
        /// если нажал ЛКМ то basecharacter передать "событие" телепорта
        /// 
        /// если нажал соответствующую кнопку на клаве (Q,W,E), 
        /// сделать проверку проверка на налицие соответствующего оружия из inventar
        /// и вызвать то basecharacter.InputSpel(spel type) 
        /// иначе ничего не делать
        //конец условий


        /// выполняет поиск текущей цели, не работает если нажата Е
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

