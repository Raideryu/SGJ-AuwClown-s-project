using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterAnimations : MonoBehaviour
{
    [SerializeField]
    string attackAnimationName = "CharacterAttack", pickingUpAnimationName = "PickingUp";
    Animator charAnimator;
    BaseCharacter character;
    Vector3 lastPosition;

    private float attackAnimTime = 1;
    private float pickUpAnimTime = 1;
    bool isAttack = false;
    public float Speed
    { get { return Vector3.Distance(lastPosition, transform.position) / Time.fixedDeltaTime; } }
    
    void Start()
    {
        lastPosition = transform.position;
        charAnimator = GetComponentInChildren<Animator>();//GetComponent<Animator>();
        character = GetComponent<BaseCharacter>();
        charAnimator.SetTrigger("ResetAnim");
        //CreateAttackAnimEndEvent();
    }

    void FixedUpdate()
    {

        //Debug.Log("текущая скорость у: " + gameObject.name + " состовляет: " + Speed);
        float speedPU = Mathf.Clamp(Speed / character.agent.speed, 0, 1);
        charAnimator.SetFloat("Speed", speedPU);
        lastPosition = transform.position;
    }

    public void DieAnim() // смерть
    {
        charAnimator.ResetTrigger("Respawn");
        charAnimator.SetTrigger("Die");
        
    }

    public void SundayAnim() //воскрешение 
    {
        charAnimator.ResetTrigger("Die");
        charAnimator.SetTrigger("Respawn");
    }


    float AnimTime(string name)
    {
        float time = 1;
        AnimationClip[] clips = charAnimator.runtimeAnimatorController.animationClips;

        for (int i = 0; i < clips.Length; i++)
        {
            
            if (clips[i].name == name)
            {
                time = clips[i].length;
                break;
            }
        }
        return time;
    }


    public void StartAttack()
    {
        //string animName = "CharacterAttack";
        attackAnimTime = AnimTime(attackAnimationName) * 0.9f;

        charAnimator.SetTrigger("StartAttack");
        isAttack = true;
        StartCoroutine(WaitAttackAnimation());
    }

    public void ResetAnim()
    {
        charAnimator.SetTrigger("ResetAnim");

        StopAllCoroutines();
        if (isAttack)
            AttackAnimEnd();
    }

    public void AttackAnimEnd()
    {
        charAnimator.ResetTrigger("StartAttack");
        if (OnAttackEnd != null)
            OnAttackEnd.Invoke();
        isAttack = false;
    }
    public void StartPickUpAnim()
    {
        pickUpAnimTime = AnimTime(pickingUpAnimationName)/8;

        // анимация подбора
        charAnimator.SetTrigger("PickUp");

        StartCoroutine(WaitPickUpAnimation());
        
    }

    public void PickUpAnimEnd()
    {
        charAnimator.ResetTrigger("PickUp");
        
        if (OnPickUpEnd != null)
            OnPickUpEnd.Invoke();
    }

    
    IEnumerator WaitAttackAnimation()
    {
        yield return new WaitForSeconds(attackAnimTime);
        AttackAnimEnd();
    }

    IEnumerator WaitPickUpAnimation()
    {
        yield return new WaitForSeconds(pickUpAnimTime);
        PickUpAnimEnd();
    }

    public delegate void TriggerHandle();
    public event TriggerHandle OnAttackEnd;
    public event TriggerHandle OnPickUpEnd;
}
