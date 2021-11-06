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

    private GameObject _target;
    [HideInInspector]
    public NavMeshAgent agent; // компонент, который отвечает за перемещение
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
            animations.OnAttackEnd += AttackEnd;  // событие завершения анимации атаки (по сути сам удар)
            animations.OnPickUpEnd += PickUpEnd;
        }

    }

    private void FixedUpdate()
    {
        if (!_target || isDied) return; // если нет цели или недошел до нее 


        if (_target && agent.remainingDistance <= agent.stoppingDistance ) // если растояние до цели меньше растояния действия
        {
            Action();
            
        }
    }

    public virtual void MoveToWithAction(Vector3 targetPos, GameObject target)
    { 
        if (target == this.gameObject || isDied) return; // проверка сам на себя

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
        // переместить персонажа
        agent.SetDestination(target);
    }

    protected virtual void Action()
    {
        // атаковать
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
    /// атаковать цель
    /// </summary>
    /// <param name="attackTarget">цель аттаки</param>
    protected virtual void Attack(BaseCharacter attackTarget)
    {
        if (isAttack) return;



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
        animations.DieAnim();
    }

    //воскрешение
    public void SunDay()
    {
        animations.SundayAnim();
    }
}
