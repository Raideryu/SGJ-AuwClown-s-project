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
            animations.OnAttackEnd += AttackEnd;  // событие завершения анимации атаки (по сути сам удар)
    }

    private void FixedUpdate()
    {
        if (!_target) return; // если нет цели или недошел до нее 
        

        if (_target && agent.remainingDistance <= actionRange) // если растояние до цели меньше растояния действия
        {
            Action();
        }
    }

    public virtual void MoveToWithAction(Vector3 targetPos, GameObject target)
    {
        if (target == this.gameObject) return; // проверка сам на себя

        agent.stoppingDistance = target != null ? actionRange : movingRange; // если есть цель для атаки, то разное растояние

        if (_target != target)
            animations.ResetAnim();

        Move(targetPos);
        _target = target;
    }


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
            Attack(enemy);
        }
        else if (_target.GetComponent<PickableSub>())
        {
            PickableSub pick = _target.GetComponent<PickableSub>();
            PickUpObj(pick);
        }
    }

    /// <summary>
    /// атаковать цель
    /// </summary>
    /// <param name="attackTarget">цель аттаки</param>
    protected virtual void Attack(BaseCharacter attackTarget)
    {
        if (isAttack) return;

        Vector3 lookRot = transform.position;
        lookRot.x = attackTarget.transform.position.x;
        lookRot.z = attackTarget.transform.position.z;
        transform.LookAt(lookRot);

        isAttack = true;

        // вызвать анимацию аттаки
        animations.StartAttack();
    }


    /// <summary>
    /// метод подбирания предмета 
    /// </summary>
    /// <param name="pickUp">сам предмет</param>
    protected virtual void PickUpObj(PickableSub pickUp)
    {
        Vector3 lookRot = transform.position;
        lookRot.x = pickUp.transform.position.x;
        lookRot.z = pickUp.transform.position.z;
        transform.LookAt(lookRot);

        animations.PickUpAnim();
        inventar.PickUpSub(pickUp);

        // подобрать предмет
       // Debug.Log("я: " + gameObject.name + " подбираю: " + pickUp.gameObject.name);
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
}
