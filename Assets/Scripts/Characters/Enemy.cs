using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    ChangeCursor cursor;

    public Transform player;
    public Transform[] moveSpots;
    private int randomSpot;

    private float waitTime;
    public float startWaitTime;

    public float speed;
    public float distance;
    //public int pos;
    bool angry = false;
    bool patrol = false;

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

        if (Vector3.Distance(transform.position, player.position) < distance)
        {
            angry = true;
            patrol = false;
            Vector3 lookAt = player.position;
            lookAt.y = transform.position.y;
            transform.LookAt(lookAt);
        }
        if (character.agent.remainingDistance > distance)
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
