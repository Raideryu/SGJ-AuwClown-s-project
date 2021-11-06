using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BaseCharacter : MonoBehaviour
{
    [SerializeField]
    float actionRange = 1, movingRange = 0.1f; //����������� ��������� �� ��������
    [SerializeField, Tooltip("����� �� �����")]
    float attackCDTime = 1;

    private GameObject _target;
    [HideInInspector]
    public NavMeshAgent agent; // ���������, ������� �������� �� �����������
    private CharacterAnimations animations;
    private CharacterInventar inventar;
    bool isAttack = false;
   
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animations = GetComponent<CharacterAnimations>();
        inventar = GetComponent<CharacterInventar>();

    }

    void Start()
    {
        agent.stoppingDistance = movingRange;
        
        if (animations)
            animations.OnAttackEnd += AttackEnd;  // ������� ���������� �������� ����� (�� ���� ��� ����)
    }

    private void FixedUpdate()
    {
        if (!_target) return; // ���� ��� ���� ��� ������� �� ��� 
        

        if (_target && agent.remainingDistance <= actionRange) // ���� ��������� �� ���� ������ ��������� ��������
        {
            Action();
        }
    }

    public virtual void MoveToWithAction(Vector3 targetPos, GameObject target)
    {
        if (target == this.gameObject) return; // �������� ��� �� ����

        agent.stoppingDistance = target != null ? actionRange : movingRange; // ���� ���� ���� ��� �����, �� ������ ���������

        if (_target != target)
            animations.ResetAnim();

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
        else if (_target.GetComponent<PickableSub>())
        {
            PickableSub pick = _target.GetComponent<PickableSub>();
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

        isAttack = true;

        // ������� �������� ������
        animations.StartAttack();
    }


    /// <summary>
    /// ����� ���������� �������� 
    /// </summary>
    /// <param name="pickUp">��� �������</param>
    protected virtual void PickUpObj(PickableSub pickUp)
    {
        Vector3 lookRot = transform.position;
        lookRot.x = pickUp.transform.position.x;
        lookRot.z = pickUp.transform.position.z;
        transform.LookAt(lookRot);

        animations.PickUpAnim();
        inventar.PickUpSub(pickUp);

        // ��������� �������
       // Debug.Log("�: " + gameObject.name + " ��������: " + pickUp.gameObject.name);
    }

    void AttackEnd()
    {
        StartCoroutine(AttackCD());
      //  Debug.Log("�: " + gameObject.name + " ������ ����: " + _target.gameObject.name);
    }

    IEnumerator AttackCD()
    {
        yield return new WaitForSeconds(attackCDTime);
        isAttack = false;
    }
}
