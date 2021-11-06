using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDrawer : MonoBehaviour
{
    public int maxHP;
    public int actualHP;
    public int stamina;
    public int maxStamina;
    public int skillCD;
    /*[SerializeField]
    private int numSkll;
    [SerializeField]
    private bool isActive; */
    //public int numOfHearts;
    //public Image[] imgHeart;
    public Image[] skillOnQ;
    public Image[] skillOnW;
    public Image[] skillOnE;
    public Image[] skillsKD;
    public Image staminaBar;
    public Image healthBar;

    /*private void Start()
    {
        actualHP = 100;
        maxHP = 100;
        stamina = 80;
        maxStamina = 80;
    }

    private void FixedUpdate()
    {
        DrawHP(actualHP, maxHP);
        DrawStamina(stamina, maxStamina);
        IsSkillsActive(numSkll, isActive);
    } */

    public void DrawHP(int hp, int maxhp)
    {
        actualHP = hp;
        maxHP = maxhp;
        if (actualHP > maxHP) actualHP = maxHP;
        CheckHP();
    }
    public void CheckHP () 
    {
        float newScale = (float)actualHP / (float)maxHP;
        healthBar.fillAmount = newScale;
    }

    public void DrawStamina(int stam, int maxstam)
    {
        stamina = stam;
        maxStamina = maxstam;
        if (stamina > maxStamina) stamina = maxStamina;
        CheckStam();


    }
    public void CheckStam()
    {
        float newScale = (float)stamina / (float)maxStamina;
        staminaBar.fillAmount = newScale;
    }

    public void IsSkillsActive(int numSkill, bool setActive) 
    {
        switch (numSkill) 
        {
            case 1:
                skillOnQ[0].gameObject.SetActive(setActive);
                break;
            case 2:
                skillOnW[0].gameObject.SetActive(setActive);
                break;
            case 3:
                skillOnE[0].gameObject.SetActive(setActive);
                break;
        }
    }
}
