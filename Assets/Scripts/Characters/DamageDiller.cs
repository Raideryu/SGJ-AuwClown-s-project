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

    public void GetDamage(BaseCharacter enemy)
    {
        
        int dam = weapon.damage1 * player.PowerPlayer;

        PlayerStats enemyStat = enemy.GetComponent<PlayerStats>();
        if(enemyStat != null && tag == "Weapon")
        {
            enemyStat.GetDamage(dam);
        }
        
    }
}
