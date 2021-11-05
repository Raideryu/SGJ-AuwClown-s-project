using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BaseCharacter : MonoBehaviour
{
    [SerializeField]
    float actionRange = 1f; //����������� ��������� �� ��������


    private GameObject _target;
    private NavMeshAgent agent; // ���������, ������� �������� �� �����������

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = actionRange;
    }

    private void Update()
    {
        if (!_target || !agent.isStopped) return; // ���� ��� ���� ��� ������� �� ��� 

        if((_target.transform.position - transform.position).magnitude <= actionRange) // ���� ��������� �� ���� ������ ��������� ��������
        {
            Action();
        }
    }


    public virtual void MoveToWithAction(Vector3 targetPos, GameObject target)
    {
        Move(targetPos);
        _target = target;
    }


    void Move(Vector3 target)
    {
        // ����������� ���������
        agent.SetDestination(target);
    }

    protected virtual void Action()
    {
        // ���������
        if (_target.GetComponent<BaseCharacter>())
        {
            BaseCharacter enemy = _target.GetComponent<BaseCharacter>();
            Attack(enemy);
        }
        else if (_target.GetComponent<PickUp>())
        {
            PickUp pick = _target.GetComponent<PickUp>();

        }
    }

    /// <summary>
    /// ��������� ����
    /// </summary>
    /// <param name="attackTarget">���� ������</param>
    protected virtual void Attack(BaseCharacter attackTarget)
    {
        // ������� �������� ������
        Debug.Log("�: "+ gameObject.name +"������ ����: " + attackTarget.gameObject.name);
    }


    /// <summary>
    /// ����� ���������� �������� 
    /// </summary>
    /// <param name="pickUp">��� �������</param>
    protected virtual void PickUpObj(PickUp pickUp)
    {
        // ��������� �������
        Debug.Log("�: " + gameObject.name + "��������: " + pickUp.gameObject.name);
    }


}
