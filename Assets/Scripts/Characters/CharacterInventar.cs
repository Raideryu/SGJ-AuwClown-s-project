using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventar : MonoBehaviour
{
    [SerializeField]
    Transform leftArm, rightArm, spine;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PickUpSub(PickableSub pickable)
    {
        pickable.PickUp(rightArm);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
