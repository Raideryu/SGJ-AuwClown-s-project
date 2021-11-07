using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BaseCharacter : MonoBehaviour
{
    [SerializeField]
    float actionRange = 1, movingRange = 0.1f; //минимальное расто€ние до действи€
    [SerializeField, Tooltip("врем€  ƒ атаки")]
    float attackCDTime = 1;

    

    //ссылки на компоненты
    [HideInInspector]
    public NavMeshAgent agent; // компонент, который отвечает за перемещение
    private CharacterAnimations animations;
    private CharacterInventar inventar;
    private DamageDiller dd;
    private PickableSub curentPicableTarget;

    public bool isDied = false; // перс умер

    CharacterAction currentAction = CharacterAction.None; // текуща€ задача

    bool isAttack = false; //атакует
    bool taskEnd;
    bool actionstarted = false;
    
    // текуща€ цель 
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
            animations.OnAttackEnd += AttackEnd;  // событие завершени€ анимации атаки (по сути сам удар)
            animations.OnPickUpEnd += PickUpEnd;
        }
    }

    private void FixedUpdate()
    {
        if (isDied || currentAction == CharacterAction.None) return; // если нет цели или недошел до нее 
        if (!_target)
            currentAction = CharacterAction.None;

        if (agent.remainingDistance <= agent.stoppingDistance ) // если расто€ние до цели меньше расто€ни€ действи€
            Action();
    }

    public virtual void MoveToWithAction(Vector3 targetPos, GameObject target)
    { 
        if (target == this.gameObject || isDied) return; // проверка сам на себ€ или на смерть
        
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
    protected virtual void Attack(BaseCharacter attackTarget)
    {
        if (isAttack) return;

        isAttack = true;
        // вызвать анимацию аттаки
        animations.StartAttack();
    }


    /// <summary>
    /// метод подбирани€ предмета 
    /// </summary>
    /// <param name="pickUp">сам предмет</param>
    protected virtual void StartPickUpObj(PickableSub pickUp)
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
    void AttackEnd()
    {
        StartCoroutine(AttackCD());
        
        if (_target && _target.GetComponent<BaseCharacter>())
        {
            dd.GetDamageEnemy(_target.GetComponent<BaseCharacter>());
        }
       
        //  Debug.Log("€: " + gameObject.name + " ударил цель: " + _target.gameObject.name);
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
        None,Attack,PickUp
    }
}
