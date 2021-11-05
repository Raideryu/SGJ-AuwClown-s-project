using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BaseCharacter : MonoBehaviour
{
    [SerializeField]
    float attackRange = 1; //минимальное расто€ние до атаки
    
    private NavMeshAgent agent; // компонент, который отвечает за перемещение

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void MoveAndAttack(Vector3 target)
    {
        // сделать событые "дошел до цели"
        //if((transform.position - target).magnitude > attackRange)
        //{
        //    Move(target);
        //}
        //else
        //{
        //    Attack(target);
        //}
    }

    public virtual void Move(Vector3 target)
    {
        // переместить персонажа
        agent.SetDestination(target);
    }

    /// <summary>
    /// атаковать цель
    /// </summary>
    /// <param name="attackTarget">цель аттаки</param>
    public virtual void Attack(BaseCharacter attackTarget)
    {
        // вызвать анимацию аттаки
    }
}
