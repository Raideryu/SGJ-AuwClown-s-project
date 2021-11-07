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

    

    //������ �� ����������
    [HideInInspector]
    public NavMeshAgent agent; // ���������, ������� �������� �� �����������
    private CharacterAnimations animations;
    private CharacterInventar inventar;
    private DamageDiller dd;
    private PickableSub curentPicableTarget;

    public bool isDied = false; // ���� ����

    CharacterAction currentAction = CharacterAction.None; // ������� ������

    bool isAttack = false; //�������
    bool taskEnd;
    bool actionstarted = false;
    
    // ������� ���� 
    private GameObject _target;

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
        if (!_target || isDied || currentAction == CharacterAction.None) return; // ���� ��� ���� ��� ������� �� ��� 

        if (agent.remainingDistance <= agent.stoppingDistance ) // ���� ��������� �� ���� ������ ��������� ��������
            Action();
    }

    public virtual void MoveToWithAction(Vector3 targetPos, GameObject target)
    { 
        if (target == this.gameObject || isDied) return; // �������� ��� �� ���� ��� �� ������
        
        if(!target || target.tag == "Ground")
        {
            currentAction = CharacterAction.None;
            agent.stoppingDistance = movingRange;
        }
        else if(target.TryGetComponent<BaseCharacter>(out BaseCharacter enemy))
        {
            currentAction = CharacterAction.Attack;
            agent.stoppingDistance = actionRange;
        }
        else if(target.TryGetComponent< PickableSub>(out PickableSub pick))
        {
            currentAction = CharacterAction.PickUp;
            agent.stoppingDistance = movingRange;
        }

       
        
        if (_target != target)
            animations.ResetAnim();

        Move(targetPos);
        if (target && target.tag != "Ground")
        {
            taskEnd = false;
            _target = target;
            actionstarted = true;
        }
        else _target = null;
        
    }
    

    void Move(Vector3 target)
    {
        // ����������� ���������
        agent.SetDestination(target);
    }

    protected virtual void Action()
    {
        
        // ���������
        if (_target.TryGetComponent<BaseCharacter>(out BaseCharacter enemy))
        {
            if (enemy.isDied)
            {
                _target = null;
                return;
            }
            Vector3 lookRot = transform.position;
            lookRot.x = enemy.transform.position.x;
            lookRot.z = enemy.transform.position.z;
            transform.LookAt(lookRot);

            Attack(enemy);
        }
        else if (_target.TryGetComponent<PickableSub>(out PickableSub pick) && !taskEnd && actionstarted)
        {
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


        isAttack = true;
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
        
        if (_target && _target.GetComponent<BaseCharacter>())
        {
            dd.GetDamageEnemy(_target.GetComponent<BaseCharacter>());
        }
       
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
        if (isDied) return;
        animations.DieAnim();
        isDied = true;
    }

    //�����������
    public void SunDay()
    {
        animations.SundayAnim();
        isDied = false;
    }

    public enum CharacterAction
    {
        None,Attack,PickUp
    }
}
