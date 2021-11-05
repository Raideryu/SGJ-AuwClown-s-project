using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterAnimations : MonoBehaviour
{
    [SerializeField]
    string attackAnimationName = "CharacterSimpleAttack";
    Animator charAnimator;
    BaseCharacter character;
    Vector3 lastPosition;
    public float Speed
    { get { return Vector3.Distance(lastPosition, transform.position) / Time.fixedDeltaTime; } }
    // Start is called before the first frame update
    void Start()
    {
        lastPosition = transform.position;
        charAnimator = GetComponent<Animator>();
        character = GetComponent<BaseCharacter>();
        CreateAttackAnimEndEvent();
    }

    void FixedUpdate()
    {
        //Debug.Log("������� �������� �: " + gameObject.name + " ����������: " + Speed);
        float speedPU = Mathf.Clamp(Speed/character.agent.speed,0,1);
        charAnimator.SetFloat("Speed", speedPU);
        lastPosition = transform.position;
    }

    //������� ������� ����� �������� � ����� ��������, � ��������� attackAnimationName � ������������� � AttackAnimEnd()
    private void CreateAttackAnimEndEvent()
    {
        AnimationEvent endAttackAnim = new AnimationEvent();
        AnimationClip clip;
        AnimationClip[] clips = charAnimator.runtimeAnimatorController.animationClips;

        for (int i = 0; i < clips.Length; i++)
        {

            if (clips[i].name.Contains(attackAnimationName))
            {
                clip = clips[i];
                endAttackAnim.time = clip.length;
                endAttackAnim.functionName = "AttackAnimEnd";
                Debug.Log("��������� ������� �������� �: " + gameObject.name);
                break;
            }
            else
            {
                Debug.LogWarning("����������� �������� ����� � �������: " + gameObject.name);
            }
        }
    }

    

    public void StartAttack()
    {
        charAnimator.SetTrigger("StartAttack");

    }

    public void AttackAnimEnd()
    {
        charAnimator.ResetTrigger("StartAttack");
        if (OnAttackEnd != null)
            OnAttackEnd.Invoke();
    }

    public delegate void TriggerHandle();
    public event TriggerHandle OnAttackEnd;
}
