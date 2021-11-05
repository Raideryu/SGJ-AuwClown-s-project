using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BaseCharacter : MonoBehaviour
{
    [SerializeField]
    float actionRange = 1f; //минимальное растояние до действия


    private GameObject _target;
    private NavMeshAgent agent; // компонент, который отвечает за перемещение

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = actionRange;
    }

    private void Update()
    {
        if (!_target || !agent.isStopped) return; // если нет цели или недошел до нее 

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

        }
    }

    /// <summary>
    /// атаковать цель
    /// </summary>
    /// <param name="attackTarget">цель аттаки</param>
    protected virtual void Attack(BaseCharacter attackTarget)
    {
        // вызвать анимацию аттаки
        Debug.Log("я: "+ gameObject.name +"атакую цель: " + attackTarget.gameObject.name);
    }


    /// <summary>
    /// метод подбирания предмета 
    /// </summary>
    /// <param name="pickUp">сам предмет</param>
    protected virtual void PickUpObj(PickUp pickUp)
    {
        // подобрать предмет
        Debug.Log("я: " + gameObject.name + "подбираю: " + pickUp.gameObject.name);
    }


}
