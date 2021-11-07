using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour, ICharacterHUD
{
    [SerializeField] bool isHideble=true;
    [SerializeField] float hideTime = 3;

    Canvas hpBarCanvas;
    [SerializeField] Image BarImage = null;
    // Start is called before the first frame update
    void Start()
    {
        hpBarCanvas = gameObject.GetComponentInChildren<Canvas>(true);
        if (hpBarCanvas && !BarImage)
        {
            Image[] images = hpBarCanvas.gameObject.GetComponentsInChildren<Image>(true);
            foreach(Image image in images)
            {
                if (image.tag == "HPBar")
                {
                    BarImage = image;
                    break;
                }
            }
        }

        if (hpBarCanvas && isHideble)
        {
            hpBarCanvas.gameObject.SetActive(false);
        }
       
    }

    public void UpdateBar(float percent)
    {
        if (!BarImage) return;
        percent = Mathf.Clamp(percent,0,1);
        BarImage.fillAmount = percent;


        if (BarImage)
        {
            hpBarCanvas.gameObject.SetActive(true);//BarImage.gameObject.SetActive(true);
            StartCoroutine(HideCanvas());
        }
       
    }

    IEnumerator HideCanvas()
    {
        yield return new WaitForSeconds(hideTime);
        hpBarCanvas.gameObject.SetActive(false);//BarImage.gameObject.SetActive(false);
    }

    public void DrawHP(int hp, int maxhp)
    {
        UpdateBar((float)hp/ (float)maxhp);
    }

    public void DrawStamina(int hp, int maxhp)
    {
        //throw new System.NotImplementedException();
    }

    public void DrawPower(int power)
    {
       // throw new System.NotImplementedException();
    }

    public void DrawAgility(int agility)
    {
       // throw new System.NotImplementedException();
    }

    public void IsSkillsActive(int numSkill, bool setActive)
    {
        //throw new System.NotImplementedException();
    }
}
