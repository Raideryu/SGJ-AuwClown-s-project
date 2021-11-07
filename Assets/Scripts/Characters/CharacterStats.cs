using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStats : MonoBehaviour
{
    [SerializeField]
    private int healthPlayer = 10;
    [SerializeField]
    private int maxHealthPlayer = 10;
    [SerializeField]
    private int powerPlayer = 1;
    [SerializeField]
    private int agilityPlayer = 1;
    [SerializeField]
    private int staminaPlayer = 5;
    [SerializeField]
    private int maxStaminaPlayer = 5;


    public int HealthPlayer
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

            }

            if (characterHUD != null)
                characterHUD.DrawHP(healthPlayer, MaxHealthPlayer);
            //Debug.Log("у меня: " + gameObject.name + " здоровье: " + healthPlayer);
        }
    }

    public int MaxHealthPlayer
    {
        get => maxHealthPlayer;
        set
        {
            maxHealthPlayer = value > 0 ? value : 0;

            if (characterHUD != null)
                characterHUD.DrawHP(HealthPlayer, maxHealthPlayer);

        }
    }
    public int StaminaPlayer
    {
        get => staminaPlayer;
        set
        {
            staminaPlayer = value > 0 ? value : 0;
            if (characterHUD != null)
                characterHUD.DrawPower(powerPlayer);



            if (characterHUD != null)
                characterHUD.DrawStamina(staminaPlayer, maxStaminaPlayer);
            //Debug.Log("у меня: " + gameObject.name + " здоровье: " + healthPlayer);
        }
    }

    public int MaxStaminaPlayer
    {
        get => maxStaminaPlayer;
        set
        {
            maxStaminaPlayer = value > 0 ? value : 0;

            if (characterHUD != null)
                characterHUD.DrawStamina(StaminaPlayer, maxStaminaPlayer);

        }
    }

    public int PowerPlayer
    {
        get => powerPlayer;
        set
        {
            powerPlayer = value > 0 ? value : 0;
            if (characterHUD != null)
                characterHUD.DrawPower(powerPlayer);
        }
    }


    public int AgilityPlayer
    {
        get => agilityPlayer;
        set
        {
            agilityPlayer = value > 0 ? value : 0;
            if (characterHUD != null)
                characterHUD.DrawAgility(agilityPlayer);

        }
    }

    // public Image HealthBar;

    [SerializeField]
    ICharacterHUD characterHUD;

    private BaseCharacter character;

    bool isDead = false;
    private void Start()
    {
        if (gameObject.tag == "Player")
            characterHUD = FindObjectOfType<UIDrawer>();

        character = GetComponent<BaseCharacter>();

        HealthPlayer = MaxHealthPlayer;
        StaminaPlayer = MaxStaminaPlayer;
        PowerPlayer = powerPlayer;
        AgilityPlayer = agilityPlayer;
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
