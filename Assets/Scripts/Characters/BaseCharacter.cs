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

    public int damageAttackSpel;
    public int damageProtectSpel;

    //������ �� ����������
    [HideInInspector]
    public NavMeshAgent agent; // ���������, ������� �������� �� �����������
    private CharacterAnimations animations;
    private CharacterInventar inventar;
    private DamageDiller dd;
    private PickableSub curentPicableTarget;

    public bool isDied = false; // ���� ����

    CharacterAction currentAction = CharacterAction.None; // ������� ������

    bool attackSkillCD = false;
    bool protectSKillCD = false;
   
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

        if (agent.remainingDistance <= agent.stoppingDistance) // ���� ��������� �� ���� ������ ��������� ��������
        {
            /// ��� ���� spellStarted � currentAction == ���-�� ��: AttackSpel, ProtectSpel �� ������� ��������������� �����, ����� Action()
            if (currentAction == CharacterAction.AttackSpel)
            {
                //���� spellStarted �� ������� ���� 
                if (spellStarted)
                {
                    StartAttackSpel(); //�� ������� ����� �����
                    spellStarted = false;
                }
            }
            else if (currentAction == CharacterAction.ProtectSpel)
            {
                if (spellStarted)
                {
                    StartProtectSpel(); //�� ������� ����� �����
                    spellStarted = false;
                }
            }
            else
                Action();
        }
    }

    public virtual void MoveToWithAction(Vector3 targetPos, GameObject target)
    {
        if (target == this.gameObject || isDied) return; // �������� ��� �� ���� ��� �� ������

        if (!target || target.tag == "Ground")
        {
            agent.stoppingDistance = movingRange;
            currentAction = CharacterAction.None;

        }
        else if (target.TryGetComponent<BaseCharacter>(out BaseCharacter enemy))
        {
            agent.stoppingDistance = actionRange;
            currentAction = CharacterAction.Attack;
            _enemy = enemy;
        }
        else if (target.TryGetComponent<PickableSub>(out PickableSub pick))
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
    public void InputSpel(bool isAttackSpel) // ��������� ��� ����� 
    {
        /// ���� ���������� ����� � ��, �� reeturn
        if (!isAttackSpel)
        {
            if (!protectSKillCD)
            {
                currentAction = CharacterAction.ProtectSpel;
                protectSKillCD = true;
            }
        }
        else if (!attackSkillCD)
        {
            currentAction = CharacterAction.AttackSpel;
            attackSkillCD = true;
        }
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
        if (currentAction == CharacterAction.AttackSpel)
        {
            if (_enemy)
            {
                currentAction = CharacterAction.Attack; //�������� curentAction � Attack
                damageAttackSpel = 20; // �������� ����������� ����� �����
                dd.GetDamageEnemy(_enemy);
            }
            else
                currentAction = CharacterAction.None;
        }
    }

    IEnumerator AttackSpelCD()
    {
        yield return new WaitForSeconds(inventar.currentWeapon.CDWeapon); // skillCDTime �������� �� inventar -> weapon ...
        attackSkillCD = false;
    }
    // ������ 1�� �����: �����

    // ������ 2�� �����: ������
    public void StartProtectSpel()
    {
        animations.StartProtectSpel();
    }

    public void EndProtectSpel()
    {
        StartCoroutine(ProtectSpelCD());
        if (currentAction == CharacterAction.ProtectSpel)
        {
            if (_enemy)
            {
                currentAction = CharacterAction.Attack;
                damageProtectSpel = 25;// �������� ����������� ����� �����
                dd.GetDamageEnemy(_enemy);
            }
            else
                currentAction = CharacterAction.None;
        }
    }

    IEnumerator ProtectSpelCD()
    {
        yield return new WaitForSeconds(inventar.currentSecondWeapon.CDSecondWeapon); // skillCDTime �������� �� inventar -> weapon ...
        protectSKillCD = false;
    }
    // ������ 2�� �����: �����


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

    void PickUpEnd()
    {
        if (curentPicableTarget)
            inventar.PickUpSub(curentPicableTarget);
        curentPicableTarget = null;
    }


    public void TeleportToTarget(Vector3 moveTo)
    {
        transform.position = moveTo;
        agent.SetDestination(moveTo);
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
        None, Attack, PickUp, AttackSpel, ProtectSpel, Teleport
    }
}
