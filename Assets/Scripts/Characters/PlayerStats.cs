using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private int HealthPlayer;
    [SerializeField]
    private int MaxHealthPlayer;
    [SerializeField]
    private int PowerPlayer;
    [SerializeField]
    private int MaxPowerPlayer;
    [SerializeField]
    private int AgilityPlayer;
    [SerializeField]
    private int MaxAgilityPlayer;
    [SerializeField]
    private int IntellectPlayer;
    [SerializeField]
    private int MaxIntellectPlayer;

    public Image HealthBar;
   
    void GetDamage(int damage)
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
    }
}
