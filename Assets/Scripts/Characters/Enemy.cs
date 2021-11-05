using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float agrDistance = 7;
    [SerializeField]
    private float waitTime = 1;
    [SerializeField]
    public Transform[] moveSpots;

    private Transform player;

    private int currentSpot = 0;

    EnemyState currentState = EnemyState.Patrol;

    private BaseCharacter character;

    void Awake()
    {
        character = GetComponent<BaseCharacter>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (!player) Debug.LogError("отсутствует сущность с тегом Player");

        if (moveSpots.Length > 0)
        {
            currentSpot = Random.Range(0, moveSpots.Length - 1);
        }
        else // если масив точек патрулирования пустой, то заполняем ео текущей позицией
        {
            moveSpots = new Transform[1] { transform };
        }
        StartPatrol();

    }

    void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, player.position) < agrDistance)
        {
            currentState = EnemyState.Angry;
            StopAllCoroutines();
        }
        else if(currentState == EnemyState.Angry)
        {
            StartPatrol();
        }
       
        if (moveSpots.Length > 0 && character.agent.remainingDistance < character.agent.stoppingDistance && currentState == EnemyState.Patrol)  // проверяем растояние до точки патрулирования
        {
            StartCoroutine(WaitTimer());
        }


        if (currentState == EnemyState.Angry)
        {
            Attack();
        }
    }

    void Attack()
    {
        // двигаем перса за плеером
        character.MoveToWithAction(player.position, player.gameObject);

    }

    void StartPatrol()
    {
        currentState = EnemyState.Patrol;
        currentSpot = currentSpot + 1 >= moveSpots.Length ? 0 : currentSpot + 1;
        character.MoveToWithAction(moveSpots[currentSpot].position, null);
    }

    IEnumerator WaitTimer()
    {
        yield return new WaitForSeconds(waitTime);

        StartPatrol();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, agrDistance);
    }

    enum EnemyState
    {
        Waiting, Patrol, Angry
    }
}
