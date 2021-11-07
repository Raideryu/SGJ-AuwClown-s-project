using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterAnimations : MonoBehaviour
{
    [SerializeField]
    string attackAnimationName = "CharacterAttack", pickingUpAnimationName = "PickingUp";
    [SerializeField]
    int speedLerp = 10;
    Animator charAnimator;
    BaseCharacter character;
    CharacterAudioSource audioSource;

    Vector3 lastPositiont;
    List<float> lastPointsSpeed = new List<float>();

    private float attackAnimTime = 1;
    private float pickUpAnimTime = 1;
    bool isAttack = false;
    //public float Speed
    //{ get 
    //    { 

    //        return Vector3.Distance(lastPosition, transform.position) / Time.fixedDeltaTime; 
    //    } 
    //}

    void Start()
    {
        audioSource = GetComponent<CharacterAudioSource>();

        for (int i = 0; i < speedLerp; i++)
        {
            lastPointsSpeed.Add(0);
        }
        lastPositiont = transform.position;

        charAnimator = GetComponentInChildren<Animator>();//GetComponent<Animator>();
        character = GetComponent<BaseCharacter>();
        charAnimator.SetTrigger("ResetAnim");

    }

    float Speed(Vector3 newPoint)
    {
        lastPointsSpeed.Add(Vector3.Distance(newPoint, lastPositiont) / Time.fixedDeltaTime);

        lastPointsSpeed.Remove(lastPointsSpeed[0]);
        lastPositiont = newPoint;
        float speed = 0;
        foreach (float a in lastPointsSpeed)
        {
            speed += a;
        }

        return speed / lastPointsSpeed.Count;
    }

    void FixedUpdate()
    {
        float speed = Speed(transform.position);
        float speedPU = Mathf.Clamp(speed / character.agent.speed, 0, 1);
        charAnimator.SetFloat("Speed", speedPU);
        audioSource.SpeedChange(speedPU);
    }

    public void DieAnim() // смерть
    {
        charAnimator.ResetTrigger("Respawn");
        charAnimator.SetTrigger("Die");
        audioSource.DeathSoundPlay();
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
        pickUpAnimTime = AnimTime(pickingUpAnimationName) / 8;

        // анимация подбора
        charAnimator.SetTrigger("PickUp");

        StartCoroutine(WaitPickUpAnimation());

        audioSource.PickUpSound();

        audioSource.SimpleAttackSoundPlay();
    }

    public void PickUpAnimEnd()
    {
        charAnimator.ResetTrigger("PickUp");

        if (OnPickUpEnd != null)
            OnPickUpEnd.Invoke();
    }

    public void StaartAttacSpell()
    {
        audioSource.SpecialAttackSoundPlay();
    }

    public void EndAttacSpell()
    {
        // конец супер атаки

    }

    public void StartTeleport()
    {
        audioSource.TeleportSoundPlay();
    }

    public void EndTeleport()
    {

    }

    public void StartBlokSpel()
    {
        audioSource.BlockSoundPlay();
    }
    public void EndBlokSpel()
    {

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
