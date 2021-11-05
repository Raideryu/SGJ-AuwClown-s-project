using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform point;
    public Transform player;
    public float speed;
    public float distance;
    public int pos;
    bool razvorot = true;

    bool angry = false;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position,player.position) < pos)
        {
            Patrol();
        }

        if(Vector3.Distance(transform.position,player.position)< distance)
        {
            angry = true;
        }

        

        if (angry == true)
        {
            Attack();
        }
    }

    void Attack()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }
    void Patrol()
    {
        if (transform.position.x > point.position.x + pos)
        {
            razvorot = true;
        }
        else if (transform.position.x < point.position.x + pos)
        {
            razvorot = false;
        }

        if (razvorot)
        {
            transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y + speed * Time.deltaTime, transform.position.z + speed * Time.deltaTime);
        }
        else
        {
            transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y - speed * Time.deltaTime, transform.position.z - speed * Time.deltaTime);
        }
    }
}
