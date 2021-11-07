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

    

    //ссылки на компоненты
    [HideInInspector]
    public NavMeshAgent agent; // компонент, который отвечает за перемещение
    private CharacterAnimations animations;
    private CharacterInventar inventar;
    private DamageDiller dd;
    private PickableSub curentPicableTarget;

    public bool isDied = false; // перс умер

    CharacterAction currentAction = CharacterAction.None; // текущая задача

    bool isAttack = false; //атакует
    bool taskEnd;
    bool actionstarted = false;
    
    // текущая цель 
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
            animations.OnAttackEnd += AttackEnd;  // событие завершения анимации атаки (по сути сам удар)
            animations.OnPickUpEnd += PickUpEnd;
        }
    }

    private void FixedUpdate()
    {
        if (!_target || isDied || currentAction == CharacterAction.None) return; // если нет цели или недошел до нее 

        if (agent.remainingDistance <= agent.stoppingDistance ) // если растояние до цели меньше растояния действия
            Action();
    }

    public virtual void MoveToWithAction(Vector3 targetPos, GameObject target)
    { 
        if (target == this.gameObject || isDied) return; // проверка сам на себя или на смерть
        
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
        // переместить персонажа
        agent.SetDestination(target);
    }

    protected virtual void Action()
    {
        
        // атаковать
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
    /// метод подбирания предмета 
    /// </summary>
    /// <param name="pickUp">сам предмет</param>
    protected virtual void StartPickUpObj(PickableSub pickUp)
    {

        //Vector3 lookRot = transform.position;
        //lookRot.x = pickUp.transform.position.x;
        //lookRot.z = pickUp.transform.position.z;
        //transform.LookAt(lookRot);
        taskEnd = true;
        animations.StartPickUpAnim();
        curentPicableTarget = pickUp;

        // подобрать предмет
        // Debug.Log("я: " + gameObject.name + " подбираю: " + pickUp.gameObject.name);
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
        None,Attack,PickUp
    }
}
