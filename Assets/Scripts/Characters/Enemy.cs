using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float distanceAgr = 7;
    [SerializeField]
    private float startWaitTime = 1;
    [SerializeField]
    public Transform[] moveSpots;

    private Transform player;

    private int randomSpot;

    private float waitTime;

    bool angry = false;
    bool patrol = false;

    private ChangeCursor cursor;
    private BaseCharacter character;

    void Start()
    {
        cursor = FindObjectOfType<ChangeCursor>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        character = GetComponent<BaseCharacter>();

        if (moveSpots.Length > 0)
        {
            randomSpot = Random.Range(0, moveSpots.Length - 1);
            character.MoveToWithAction(moveSpots[randomSpot].position, null);
        }
           
    }

    void Update()
    {
        if (moveSpots.Length > 0 && character.agent.remainingDistance > 0.2f) // проверяем растояние до цели
        {
            patrol = true;
            
            Vector3 look = moveSpots[randomSpot].position;
            look.y = transform.position.y;
            transform.LookAt(look);
        }

        if (Vector3.Distance(transform.position, player.position) < distanceAgr)
        {
            angry = true;
            patrol = false;
            Vector3 lookAt = player.position;
            lookAt.y = transform.position.y;
            transform.LookAt(lookAt);
        }
        if (character.agent.remainingDistance > distanceAgr)
        {
            angry = false;
        }

        if (angry == true)
        {
            Attack();
        }

        if (patrol == true)
        {
            Chill();
        }
    }

    void Attack()
    {
        // двигаем перса за плеером
        character.MoveToWithAction(player.position, player.gameObject);
        
    }

    void Chill()
    {
        // двигаем перса туда
        character.MoveToWithAction(moveSpots[randomSpot].position, null); 


        if (waitTime <= 0)
        {
            randomSpot = Random.Range(0, moveSpots.Length);
            waitTime = startWaitTime;
        }
        else
        {
            waitTime -= Time.deltaTime;
        }
    }

    private void OnMouseEnter()
    {
        if (cursor)
        {
            cursor.SetCursor(tag);
        }
        else
        {
            Debug.LogError("отсутствует на сцене курсор");
        }
    }

    private void OnMouseExit()
    {
        if (cursor)
        {
            cursor.ResetCursor();
        }
        else
        {
            Debug.LogError("отсутствует на сцене курсор");
        }
    }
}
