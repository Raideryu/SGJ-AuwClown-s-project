using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterHUD
{
    public void DrawHP(int hp, int maxhp);
    public void DrawStamina(int hp, int maxhp);
    public void DrawPower(int power);

    public void DrawAgility(int agility);

    public void IsSkillsActive(int numSkill, bool setActive);
}
