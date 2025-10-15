using UnityEngine;

public class BattleCat : MonoBehaviour
{
    [Header("Trun")]
    public int myTrun = 0;

    [Header("Stat")]
    public string catName;
    [Space(7f)]
    public int battleCatLevel;
    public float battleCatTurnSpeed;
    public int battleCatHP;
    public int battleCatATK;
    public int battleCatDEF;
    [Space(7f)]
    public int skill1Lv;
    public int skill2Lv;
    public int skill3Lv;
    [Space(7f)]
    public int skill1Number;
    public int skill2Number;
    public int skill3Number;


    [Space(15f)]
    public CharacterData charData;


    public void LoadCharacterData()
    {
        catName = charData.patName;
        battleCatLevel = charData.showLevel;
        battleCatTurnSpeed = charData.speed;
        battleCatATK = charData.atk;
        battleCatDEF = charData.def;
        battleCatHP = charData.maxHp;
        skill1Lv = charData.skill1Lv;
        skill2Lv = charData.skill2Lv;
        skill3Lv = charData.skill3Lv;
        skill1Number = charData.skill1Number;
        skill2Number = charData.skill2Number;
        skill3Number = charData.skill3Number;
    }

    public void MyTurnStart()
    {
        myTrun++;
    }

    public void MyTurnEnd()
    {
        
    }

    public void StatUpdata()
    {

    }

    public void TakeDamage(int otherDamage)
    {
        int damage = otherDamage - (int)((((battleCatDEF + battleCatLevel) * 0.03f) + 1) * 0.001f) * otherDamage;
        battleCatHP -= damage;
    }

    public void UseSkill(int skillNumber)
    {
        switch(skillNumber)
        {
            case 1:
                charData.Skill1();
                break;
            case 2:
                charData.Skill2();
                break;
            case 3:
                charData.Skill3();
                break;
        }
    }
}
