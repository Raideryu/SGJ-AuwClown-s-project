using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BaseCharacter : MonoBehaviour
{
    [SerializeField]
    float actionRange = 1; //минимальное растояние до действия
    [SerializeField, Tooltip("время КД атаки")]
    float attackCDTime = 1;

    private GameObject _target;
    private NavMeshAgent agent; // компонент, который отвечает за перемещение
    private CharacterAnimations animations;

    bool isAttack=false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = actionRange;
        animations = GetComponent<CharacterAnimations>();

        if (animations)
        {
            animations.OnAttackEnd += AttackEnd;  // событие завершения анимации атаки (по сути сам удар)
        }
    }

    private void Update()
    {
        if (!_target) return; // если нет цели или недошел до нее 
        Debug.Log(" текущая цель: " + _target.name);
        if((_target.transform.position - transform.position).magnitude <= actionRange) // если растояние до цели меньше растояния действия
        {
            Action();
        }
    }


    public virtual void MoveToWithAction(Vector3 targetPos, GameObject target)
    {
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
        else if (_target.GetComponent<PickUp>())
        {
            PickUp pick = _target.GetComponent<PickUp>();
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
        isAttack = true;

        // вызвать анимацию аттаки
        Debug.Log("я: "+ gameObject.name +" атакую цель: " + attackTarget.gameObject.name);
        animations.StartAttack();
    }


    /// <summary>
    /// метод подбирания предмета 
    /// </summary>
    /// <param name="pickUp">сам предмет</param>
    protected virtual void PickUpObj(PickUp pickUp)
    {
        // подобрать предмет
        Debug.Log("я: " + gameObject.name + " подбираю: " + pickUp.gameObject.name);
    }

    void AttackEnd()
    {
        StartCoroutine(AttackCD());
        Debug.Log("я: " + gameObject.name + " ударил цель: " + _target.gameObject.name);
    }

    IEnumerator AttackCD()
    {
        yield return new WaitForSeconds(attackCDTime);
        isAttack = false;
    }
}
