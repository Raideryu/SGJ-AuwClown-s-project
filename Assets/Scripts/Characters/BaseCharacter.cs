using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BaseCharacter : MonoBehaviour
{
    [SerializeField]
    float attackRange = 1; //����������� ��������� �� �����
    
    private NavMeshAgent agent; // ���������, ������� �������� �� �����������

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void MoveAndAttack(Vector3 target)
    {
        // ������� ������� "����� �� ����"
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
        // ����������� ���������
        agent.SetDestination(target);
    }

    /// <summary>
    /// ��������� ����
    /// </summary>
    /// <param name="attackTarget">���� ������</param>
    public virtual void Attack(BaseCharacter attackTarget)
    {
        // ������� �������� ������
    }
}
