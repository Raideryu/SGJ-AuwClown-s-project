using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDiller : MonoBehaviour
{
    public int skillModifire = 1;

    CharacterStats characterStats;
    CharacterInventar inventar;
    int WeaponDamage
    {
        get
        {
            if (inventar.currentWeapon)
                return inventar.currentWeapon.damage;
            else
                return 0;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        characterStats = GetComponent<CharacterStats>();
        inventar = GetComponent<CharacterInventar>();
    }

    public void GetDamageEnemy(BaseCharacter enemy)
    {
        int dam = WeaponDamage * characterStats.PowerPlayer;

        int resDamage = (dam + characterStats.PowerPlayer) * skillModifire;
        CharacterStats enemyStat = enemy.GetComponent<CharacterStats>();

        enemyStat.GetDamage(resDamage);
    }
}
