using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDiller : MonoBehaviour
{
    public int skillModifire = 1;

    //public GameObject selectCircle = null;

    //public bool cast = false;

    //private Ray p_ray;
    //private RaycastHit p_hit;
    //private Camera p_camera = null;

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

    void Start()
    {
        //p_camera = Camera.main;
        //selectCircle.SetActive(false);
        characterStats = GetComponent<CharacterStats>();
        inventar = GetComponent<CharacterInventar>();
    }

    // перенести в playerInput
    //void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.Q))
    //    {
    //        cast = true;
    //    }
    //    if (Input.GetKeyDown(KeyCode.E))
    //    {
    //        cast = true;
    //    }

    //    if (cast)
    //    {
    //        if(Physics.Raycast(p_ray,out p_hit))
    //        {
    //            if(rayMousePosition("Ground"))
    //            {
    //                selectCircle.transform.position = new Vector3(p_hit.point.x, p_hit.point.y + 0.5f, p_hit.point.z);
    //                selectCircle.SetActive(true);
    //            }
    //        }
    //        if(Input.GetMouseButton(1))
    //        {
    //            cast = false;
    //            selectCircle.SetActive(false);
    //        }
           
    //    }
    //    else { selectCircle.SetActive(false); }
    //}

    //private bool rayMousePosition(string _tag)
    //{
    //    p_ray = p_camera.ScreenPointToRay(Input.mousePosition);
    //    if (Physics.Raycast(p_ray, out p_hit) && p_hit.collider.CompareTag(_tag))
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    public void GetDamageEnemy(BaseCharacter enemy)
    {
        int dam = WeaponDamage * characterStats.PowerPlayer;

        int resDamage = (dam + characterStats.PowerPlayer) * skillModifire;
        CharacterStats enemyStat = enemy.GetComponent<CharacterStats>();

        enemyStat.GetDamage(resDamage);
    }
}
