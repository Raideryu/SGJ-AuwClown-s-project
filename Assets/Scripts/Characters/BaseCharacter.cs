using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BaseCharacter : MonoBehaviour
{
    [SerializeField]
    float actionRange = 1; //����������� ��������� �� ��������
    [SerializeField, Tooltip("����� �� �����")]
    float attackCDTime = 1;

    private GameObject _target;
    private NavMeshAgent agent; // ���������, ������� �������� �� �����������
    private CharacterAnimations animations;

    bool isAttack=false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = actionRange;
        animations = GetComponent<CharacterAnimations>();

        if (animations)
        {
            animations.OnAttackEnd += AttackEnd;  // ������� ���������� �������� ����� (�� ���� ��� ����)
        }
    }

    private void Update()
    {
        if (!_target) return; // ���� ��� ���� ��� ������� �� ��� 
        Debug.Log(" ������� ����: " + _target.name);
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
            PickUpObj(pick);
        }
    }

    /// <summary>
    /// ��������� ����
    /// </summary>
    /// <param name="attackTarget">���� ������</param>
    protected virtual void Attack(BaseCharacter attackTarget)
    {
        if (isAttack) return;
        isAttack = true;

        // ������� �������� ������
        Debug.Log("�: "+ gameObject.name +" ������ ����: " + attackTarget.gameObject.name);
        animations.StartAttack();
    }


    /// <summary>
    /// ����� ���������� �������� 
    /// </summary>
    /// <param name="pickUp">��� �������</param>
    protected virtual void PickUpObj(PickUp pickUp)
    {
        // ��������� �������
        Debug.Log("�: " + gameObject.name + " ��������: " + pickUp.gameObject.name);
    }

    void AttackEnd()
    {
        StartCoroutine(AttackCD());
        Debug.Log("�: " + gameObject.name + " ������ ����: " + _target.gameObject.name);
    }

    IEnumerator AttackCD()
    {
        yield return new WaitForSeconds(attackCDTime);
        isAttack = false;
    }
}
