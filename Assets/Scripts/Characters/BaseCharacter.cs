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
    //[SerializeField, Tooltip("����� �� �����������")]
    float skillCDTime = 3;


    //������ �� ����������
    [HideInInspector]
    public NavMeshAgent agent; // ���������, ������� �������� �� �����������
    private CharacterAnimations animations;
    private CharacterInventar inventar;
    private DamageDiller dd;
    private PickableSub curentPicableTarget;

    public bool isDied = false; // ���� ����

    CharacterAction currentAction = CharacterAction.None; // ������� ������

    bool isSkill = false; // ���������� �����������
    bool isAttack = false; //�������
    bool taskEnd;
    bool actionstarted = false;
    bool spellStarted = false;
    // ������� ���� 
    private GameObject _target;
    private BaseCharacter _enemy;
    private PickableSub _pick;
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
        if (isDied || currentAction == CharacterAction.None) return; // ���� ��� ���� ��� ������� �� ��� 
        if (!_target)
            currentAction = CharacterAction.None;

        if (agent.remainingDistance <= agent.stoppingDistance ) // ���� ��������� �� ���� ������ ��������� ��������
            /// ��� ���� spellStarted � currentAction == ���-�� ��: AttackSpel, ProtectSpel �� ������� ��������������� �����, ����� Action()
            if(currentAction == CharacterAction.AttackSpel || currentAction == CharacterAction.ProtectSpel)
            {
                //���� spellStarted �� ������� ���� 
                if (spellStarted)
                {
                    //�� ������� ����� �����
                    spellStarted = false;
                }
            }
            Action();
    }

    public virtual void MoveToWithAction(Vector3 targetPos, GameObject target)
    { 
        if (target == this.gameObject || isDied) return; // �������� ��� �� ���� ��� �� ������
        
        if(!target || target.tag == "Ground")
        {
            agent.stoppingDistance = movingRange;
            currentAction = CharacterAction.None;
            
        }
        else if(target.TryGetComponent<BaseCharacter>(out BaseCharacter enemy))
        {
            agent.stoppingDistance = actionRange;
            currentAction = CharacterAction.Attack;
            _enemy = enemy;
        }
        else if(target.TryGetComponent< PickableSub>(out PickableSub pick))
        {
            agent.stoppingDistance = movingRange;
            currentAction = CharacterAction.PickUp;
            _pick = pick;
            actionstarted = true;
        }

        if (_target != target)
            animations.ResetAnim();

        Move(targetPos);

        if (target && target.tag != "Ground")
        {
           // taskEnd = false;
            _target = target;
            //actionstarted = true;
        }
        else _target = null;
        
    }
    // ������ ���������� �� player input
    public void InputSpel() // ��������� ��� ����� 
    {
        /// ���� ���������� ����� � ��, �� reeturn

        // � ����������� �� ���� ����� �����
        /// curentAction -> AttackSpel, ProtectSpel
        spellStarted = true;
    }
    void Move(Vector3 target)
    {
        // ����������� ��������� 
        agent.SetDestination(target);
    }

    protected virtual void Action()
    {
        // ���������
        if (currentAction == CharacterAction.Attack)
        {
            if (_enemy.isDied)
            {
                _target = null;
                return;
            }

            Vector3 lookRot = _enemy.transform.position;
            lookRot.y = transform.position.y;
            transform.LookAt(lookRot);

            Attack(_enemy);
        }
        else if (currentAction == CharacterAction.PickUp && actionstarted)
        {
            if (_pick != curentPicableTarget)
            {
                StartPickUpObj(_pick);
                actionstarted = false;
            }
            
        }
    }

    /// <summary>
    /// ��������� ����
    /// </summary>
    /// <param name="attackTarget">���� ������</param>
     void Attack(BaseCharacter attackTarget)
    {
        if (isAttack) return;

        isAttack = true;
        // ������� �������� ������
        animations.StartAttack();
    }

    // ������ 1�� �����: ������
       
    public void StartAttackSpel()
    {
        
        animations.StaartAttacSpell();
    }
    public void EndAttackSpel()
    {
        StartCoroutine(AttackSpelCD());
        if(currentAction == CharacterAction.AttackSpel)
        if (_enemy)
        {
            // �������� ����������� ����� �����
            dd.GetDamageEnemy(_enemy);
        }

        //�������� curentAction � Attack
        currentAction = CharacterAction.AttackSpel;
    }

    IEnumerator AttackSpelCD()
    {
        // ������ ����� ���� ����, ���� �� ������� ��
        yield return new WaitForSeconds(skillCDTime); // skillCDTime �������� �� inventar -> weapon ...

        isSkill = false;
        // ����� ������ �� ��������� �������������
    }
    // ������ 1�� �����: �����
    // ���������� ��� ��������� �����

    /// <summary>
    /// ����� ���������� �������� 
    /// </summary>
    /// <param name="pickUp">��� �������</param>
    void StartPickUpObj(PickableSub pickUp)
    {
        //taskEnd = true;
        animations.StartPickUpAnim();
        curentPicableTarget = pickUp;

    }

    // ������ 1�� �����: ������
    void PickUpEnd()
    {
        if (curentPicableTarget)
            inventar.PickUpSub(curentPicableTarget);
        curentPicableTarget = null;
    }
   
    
    public void TeleportToTarget(Vector3 moveTo)
    {
        transform.position = moveTo;
        animations.StartTeleport();
        inventar.DestroySkroll();
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
        None,Attack,PickUp, AttackSpel, ProtectSpel, Teleport
    }
}
