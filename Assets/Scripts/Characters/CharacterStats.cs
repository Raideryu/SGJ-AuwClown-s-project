using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStats : MonoBehaviour
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

            Debug.Log("у меня: " + gameObject.name + " здоровье: " + healthPlayer);
        }
    }
    [SerializeField]
    private int MaxHealthPlayer =2;
    [SerializeField]
    int powerPlayer = 1;

    public int PowerPlayer
    {
        get => powerPlayer;
        set
        {
            if (value <= 0)
            {
                powerPlayer = 0;
            }
            else
            {
                powerPlayer = value;
            }
        }
    }
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

   // public Image HealthBar;

    private BaseCharacter character;
    bool isDead = false;
    private void Start()
    {
        character = GetComponent<BaseCharacter>();
    }

   public void GetDamage(int damage)
    {
        HealthPlayer -= damage;

    }

    void GetHealth(int bonusHealth)
    {
        HealthPlayer += bonusHealth;

        if (HealthPlayer > MaxHealthPlayer)
        {
            HealthPlayer = MaxHealthPlayer;
        }
    }
    void Die()
    {
        if (isDead) return;
        character.Die();
    }
}
