using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawerUI 
{
    public void DrawHP () 
    {

    }
}

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private int healthPlayer =10;
     int HealthPlayer  
    {
        get => healthPlayer; 
        set 
        {
            if (value <= 0)
            {
                healthPlayer = 0;
                Die();
            }
            else
            { 
                healthPlayer = value;
                // тут обновить HUD
            }
        }
    }
    [SerializeField]
    private int MaxHealthPlayer =2;
    [SerializeField]
    private int PowerPlayer = 1;
    [SerializeField]
    private int MaxPowerPlayer = 2;
    [SerializeField]
    private int AgilityPlayer = 1;
    [SerializeField]
    private int MaxAgilityPlayer = 2;
    [SerializeField]
    private int IntellectPlayer = 1;
    [SerializeField]
    private int MaxIntellectPlayer = 2;

    public Image HealthBar;

    private BaseCharacter character;

    private void Start()
    {
        character = GetComponent<BaseCharacter>();
    }

   public void GetDamage(int damage)
    {
        HealthPlayer -= damage;
        HealthBar.fillAmount = HealthPlayer * 0.1f;
        if(HealthPlayer <= 0)
        {
            Debug.Log("Умер персонаж: " + gameObject.name);
            Die();
        }
    }

    void GetHealth(int bonusHealth)
    {
        HealthPlayer += bonusHealth;
        HealthBar.fillAmount = HealthPlayer * 0.1f;
        if (HealthPlayer > MaxHealthPlayer)
        {
            HealthPlayer = MaxHealthPlayer;
        }
    }
    void Die()
    {
        character.Die();
    }
}
