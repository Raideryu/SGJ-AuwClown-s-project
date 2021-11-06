using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDiller : MonoBehaviour
{
    PlayerStats player;
    Weapons weapon;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GetDamagePlayer(BaseCharacter player)
    {
        int dam = 2;

        PlayerStats playerStat = player.GetComponent<PlayerStats>();

        if(playerStat != null)
        {
            playerStat.GetDamage(dam);
        }    
    }
    public void GetDamageEnemy(BaseCharacter enemy)
    {

        int dam = weapon.damage1 * player.powerPlayer;

        PlayerStats enemyStat = enemy.GetComponent<PlayerStats>();

        if (enemyStat != null)
        {
            enemyStat.GetDamage(dam);
        }
    }
}
