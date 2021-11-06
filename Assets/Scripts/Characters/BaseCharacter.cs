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
    public bool isDied=false;
    bool taskEnd;
    DamageDiller dd;
    PickableSub curentPicableTarget;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animations = GetComponent<CharacterAnimations>();
        inventar = GetComponent<CharacterInventar>();

        dd = GetComponent<DamageDiller>();
    }

    void Start()
    {
        agent.stoppingDistance = movingRange;

        if (animations)
        {
            animations.OnAttackEnd += AttackEnd;  // ������� ���������� �������� ����� (�� ���� ��� ����)
            animations.OnPickUpEnd += PickUpEnd;
        }

    }

    private void FixedUpdate()
    {
        if (!_target || isDied) return; // ���� ��� ���� ��� ������� �� ��� 


        if (_target && agent.remainingDistance <= agent.stoppingDistance ) // ���� ��������� �� ���� ������ ��������� ��������
        {
            Action();
            
        }
    }

    public virtual void MoveToWithAction(Vector3 targetPos, GameObject target)
    { 
        if (target == this.gameObject || isDied) return; // �������� ��� �� ����

        if (target && target.gameObject.GetComponent<BaseCharacter>())
            agent.stoppingDistance = actionRange;
        else agent.stoppingDistance = movingRange;

        taskEnd = false;

        if (_target != target)
            animations.ResetAnim();

        Move(targetPos);
        _target = target;
        actionstarted = true;
    }
    bool actionstarted = false;

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

            Vector3 lookRot = transform.position;
            lookRot.x = enemy.transform.position.x;
            lookRot.z = enemy.transform.position.z;
            transform.LookAt(lookRot);


            Attack(enemy);
        }
        else if (_target.GetComponent<PickableSub>() && !taskEnd && actionstarted)
        {
            PickableSub pick = _target.GetComponent<PickableSub>();
            if (pick != curentPicableTarget)
            StartPickUpObj(pick);
            actionstarted = false; 
        }
    }

    /// <summary>
    /// ��������� ����
    /// </summary>
    /// <param name="attackTarget">���� ������</param>
    protected virtual void Attack(BaseCharacter attackTarget)
    {
        if (isAttack) return;



        // ������� �������� ������
        animations.StartAttack();
    }


    /// <summary>
    /// ����� ���������� �������� 
    /// </summary>
    /// <param name="pickUp">��� �������</param>
    protected virtual void StartPickUpObj(PickableSub pickUp)
    {

        //Vector3 lookRot = transform.position;
        //lookRot.x = pickUp.transform.position.x;
        //lookRot.z = pickUp.transform.position.z;
        //transform.LookAt(lookRot);
        taskEnd = true;
        animations.StartPickUpAnim();
        curentPicableTarget = pickUp;

        // ��������� �������
        // Debug.Log("�: " + gameObject.name + " ��������: " + pickUp.gameObject.name);
    }

    void PickUpEnd()
    {
        if (curentPicableTarget)
            inventar.PickUpSub(curentPicableTarget);
        curentPicableTarget = null;
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

    //������
    public void Die()
    {
        animations.DieAnim();
    }

    //�����������
    public void SunDay()
    {
        animations.SundayAnim();
    }
}
