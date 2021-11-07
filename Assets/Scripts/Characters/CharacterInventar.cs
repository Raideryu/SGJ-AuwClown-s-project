using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventar : MonoBehaviour
{
    [SerializeField]
    Transform leftArm, rightArm, spine;
    [SerializeField]
    public Weapons currentWeapon;
    [SerializeField]
    public SecondWeapon currentSecondWeapon;
    [SerializeField]
    public Scroll currentScroll;
    // Start is called before the first frame update
    void Start()
    {
        //if(currentWeapon)
        if (currentWeapon)
        {
            currentWeapon = Instantiate(currentWeapon);
            currentWeapon.PickUp(rightArm);
        }
        if (currentSecondWeapon)
        {
            currentSecondWeapon = Instantiate(currentSecondWeapon);
            currentSecondWeapon.PickUp(leftArm);
        }
        if (currentScroll)
        {
            currentScroll = Instantiate(currentScroll);
            currentScroll.PickUp(spine);
        }
    }

    public void PickUpSub(PickableSub pickable)
    {
        if (pickable == currentWeapon || pickable == currentSecondWeapon || pickable == currentScroll) return;

        if ((pickable as Weapons) != null)
        {
            if (currentWeapon != null)
            {
                currentWeapon.Drop();
            }
            pickable.PickUp(rightArm);
            currentWeapon = (Weapons)pickable;

        }
        else if ((pickable as SecondWeapon) != null)
        {
            if (currentSecondWeapon != null)
            {
                currentSecondWeapon.Drop();
            }
            
            currentSecondWeapon = (SecondWeapon)pickable;

            pickable.PickUp(leftArm);
        }
        else if ((pickable as Scroll) != null)
        {
            if (currentScroll != null)
            {
                currentScroll.Drop();
            }

            currentScroll = (Scroll)pickable;
            pickable.PickUp(spine);
        }

        //Weapons w = pickable.GetComponent<Weapons>();

    }

    // Update is called once per frame
    void Update()
    {

    }
}
