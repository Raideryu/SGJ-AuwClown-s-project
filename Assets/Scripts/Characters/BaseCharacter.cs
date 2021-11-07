using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BaseCharacter : MonoBehaviour
{
    [SerializeField]
    float actionRange = 1, movingRange = 0.1f; //минимальное растояние до действия
    [SerializeField, Tooltip("время КД атаки")]
    float attackCDTime = 1;

    public int damageAttackSpel;
    public int damageProtectSpel;

    //ссылки на компоненты
    [HideInInspector]
    public NavMeshAgent agent; // компонент, который отвечает за перемещение
    private CharacterAnimations animations;
    private CharacterInventar inventar;
    private DamageDiller dd;
    private PickableSub curentPicableTarget;

    public bool isDied = false; // перс умер

    CharacterAction currentAction = CharacterAction.None; // текущая задача

    bool attackSkillCD = false;
    bool protectSKillCD = false;
   
    bool isAttack = false; //атакует
    bool taskEnd;
    bool actionstarted = false;
    bool spellStarted = false;
    // текущая цель 
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
            animations.OnAttackEnd += AttackEnd;  // событие завершения анимации атаки (по сути сам удар)
            animations.OnPickUpEnd += PickUpEnd;
        }
    }

    private void FixedUpdate()
    {
        if (isDied || currentAction == CharacterAction.None) return; // если нет цели или недошел до нее 
        if (!_target)
            currentAction = CharacterAction.None;

        if (agent.remainingDistance <= agent.stoppingDistance) // если растояние до цели меньше растояния действия
        {
            /// тут если spellStarted и currentAction == что-то из: AttackSpel, ProtectSpel то вызвать соответствующий метод, иначе Action()
            if (currentAction == CharacterAction.AttackSpel)
            {
                //если spellStarted то вызывай дибо 
                if (spellStarted)
                {
                    StartAttackSpel(); //то вызывай метод скила
                    spellStarted = false;
                }
            }
            else if (currentAction == CharacterAction.ProtectSpel)
            {
                if (spellStarted)
                {
                    StartProtectSpel(); //то вызывай метод скила
                    spellStarted = false;
                }
            }
            else
                Action();
        }
    }

    public virtual void MoveToWithAction(Vector3 targetPos, GameObject target)
    {
        if (target == this.gameObject || isDied) return; // проверка сам на себя или на смерть

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
    // должен вызываться из player input
    public void InputSpel(bool isAttackSpel) // параметры тип скила 
    {
        /// если вызываемый скилл в кд, то reeturn
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
        // в зависимости от типа скила задаь
        /// curentAction -> AttackSpel, ProtectSpel
        spellStarted = true;
    }
    void Move(Vector3 target)
    {
        // переместить персонажа 
        agent.SetDestination(target);
    }

    protected virtual void Action()
    {
        // атаковать
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
    /// атаковать цель
    /// </summary>
    /// <param name="attackTarget">цель аттаки</param>
    void Attack(BaseCharacter attackTarget)
    {
        if (isAttack) return;

        isAttack = true;
        // вызвать анимацию аттаки
        animations.StartAttack();
    }

    // методы 1го скила: начало

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
                currentAction = CharacterAction.Attack; //сбросить curentAction в Attack
                damageAttackSpel = 20; // добавить модификатор урона скила
                dd.GetDamageEnemy(_enemy);
            }
            else
                currentAction = CharacterAction.None;
        }
    }

    IEnumerator AttackSpelCD()
    {
        yield return new WaitForSeconds(inventar.currentWeapon.CDWeapon); // skillCDTime получать из inventar -> weapon ...
        attackSkillCD = false;
    }
    // методы 1го скила: конец

    // методы 2го скила: начало
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
                damageProtectSpel = 25;// добавить модификатор урона скила
                dd.GetDamageEnemy(_enemy);
            }
            else
                currentAction = CharacterAction.None;
        }
    }

    IEnumerator ProtectSpelCD()
    {
        yield return new WaitForSeconds(inventar.currentSecondWeapon.CDSecondWeapon); // skillCDTime получать из inventar -> weapon ...
        protectSKillCD = false;
    }
    // методы 2го скила: конец


    /// <summary>
    /// метод подбирания предмета 
    /// </summary>
    /// <param name="pickUp">сам предмет</param>
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

        //  Debug.Log("я: " + gameObject.name + " ударил цель: " + _target.gameObject.name);
    }

    IEnumerator AttackCD()
    {
        yield return new WaitForSeconds(attackCDTime);
        isAttack = false;
    }

    //смерть
    public void Die()
    {
        if (isDied) return;
        animations.DieAnim();
        isDied = true;
    }

    //воскрешение
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
