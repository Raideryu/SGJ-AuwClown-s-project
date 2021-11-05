using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterAnimations : MonoBehaviour
{
    Animator charAnimator;
    // Start is called before the first frame update
    void Start()
    {
        charAnimator = GetComponent<Animator>();
    }

    public void StartAttack()
    {
        charAnimator.SetTrigger("StartAttack");
    }

    public void AttackAnimEnd()
    {
        if (OnAttackEnd != null)
            OnAttackEnd.Invoke();
    }

    public delegate void TriggerHandle();
    public event TriggerHandle OnAttackEnd;
}
