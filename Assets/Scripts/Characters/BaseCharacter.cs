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
    public NavMeshAgent agent; // ���������, ������� �������� �� �����������
    private CharacterAnimations animations;

    bool isAttack = false;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
       // agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = 0;
        animations = GetComponent<CharacterAnimations>();

        if (animations)
        {
            animations.OnAttackEnd += AttackEnd;  // ������� ���������� �������� ����� (�� ���� ��� ����)
        }
    }

    private void Update()
    {
        
    }
    
    private void FixedUpdate()
    {
        if (!_target) return; // ���� ��� ���� ��� ������� �� ��� 
        Debug.Log(" ������� ����: " + _target.name);

        if (_target && agent.remainingDistance <= actionRange) // ���� ��������� �� ���� ������ ��������� ��������
        {
            Action();
        }
    }

    public virtual void MoveToWithAction(Vector3 targetPos, GameObject target)
    {
        if (target == this.gameObject) return;// �������� ��� �� ����

        agent.stoppingDistance = target != null ? actionRange : 0;
        

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

        Vector3 lookRot = transform.position;
        lookRot.x = attackTarget.transform.position.x;
        lookRot.z = attackTarget.transform.position.z;
        transform.LookAt(lookRot);
        //transform.rotation = Quaternion.LookRotation(lookRot);
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
        Vector3 lookRot = transform.position;
        lookRot.x = pickUp.transform.position.x;
        lookRot.z = pickUp.transform.position.z;
        transform.LookAt(lookRot);
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
