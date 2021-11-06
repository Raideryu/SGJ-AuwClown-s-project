using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterAnimations : MonoBehaviour
{
    [SerializeField]
    string attackAnimationName = "CharacterAttack";
    Animator charAnimator;
    BaseCharacter character;
    Vector3 lastPosition;
    public float Speed
    { get { return Vector3.Distance(lastPosition, transform.position) / Time.fixedDeltaTime; } }
    // Start is called before the first frame update
    void Start()
    {
        lastPosition = transform.position;
        charAnimator = GetComponentInChildren<Animator>();//GetComponent<Animator>();
        character = GetComponent<BaseCharacter>();
        CreateAttackAnimEndEvent();
    }

    void FixedUpdate()
    {
      
        //Debug.Log("текущая скорость у: " + gameObject.name + " состовляет: " + Speed);
        float speedPU = Mathf.Clamp(Speed/character.agent.speed,0,1);
        charAnimator.SetFloat("Speed", speedPU);
        lastPosition = transform.position;
    }

    //создает событие конца анимации в конце анимации, с названием attackAnimationName и привязывается к AttackAnimEnd()
    private void CreateAttackAnimEndEvent()
    {
        AnimationEvent endAttackAnim = new AnimationEvent();
        AnimationClip[] clips = charAnimator.runtimeAnimatorController.animationClips;

    }

    float AttackAnimTime(string name)
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
        string animName = "CharacterAttack";
        attackAnimTime = AttackAnimTime(animName);

        charAnimator.SetTrigger("StartAttack");

        StartCoroutine(WaitAttackAnimation());
    }

    public void ResetAnim()
    {
        charAnimator.SetTrigger("ResetAnim");
        StopAllCoroutines();
    }

    public void AttackAnimEnd()
    {
        //charAnimator.ResetTrigger("StartAttack");
        if (OnAttackEnd != null)
            OnAttackEnd.Invoke();
    }

    public void PickUpAnim()
    {
        // анимация подбора
    }

    private float attackAnimTime = 1;
    IEnumerator WaitAttackAnimation()
    {
        yield return new WaitForSeconds(attackAnimTime);
        AttackAnimEnd();
    }

    public delegate void TriggerHandle();
    public event TriggerHandle OnAttackEnd;
}
