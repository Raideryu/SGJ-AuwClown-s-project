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
    public int pos;
    bool angry = false;
    bool patrol = false;

    private BaseCharacter character;

    void Start()
    {
        cursor = FindObjectOfType<ChangeCursor>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        randomSpot = Random.Range(0, moveSpots.Length);

        character = GetComponent<BaseCharacter>();
    }

    void Update()
    {
        
        if (Vector3.Distance(transform.position,moveSpots[randomSpot].position) > 0.2f)
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
        if (Vector3.Distance(transform.position, player.position) > distance)
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
        character.MoveToWithAction(player.position, player.gameObject);
        // transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    void Chill()
    {
        //transform.position = Vector3.MoveTowards(transform.position, moveSpots[randomSpot].position, speed * Time.deltaTime);

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
