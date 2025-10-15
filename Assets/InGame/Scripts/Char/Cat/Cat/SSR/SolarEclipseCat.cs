using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarEclipseCat : CharacterData
{
    //ÀÏ½Ä
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    protected override void StatisticsUp()
    {
        atk += Random.Range(5, 8);
        def += Random.Range(2, 4);
        maxHp += Random.Range(21, 23);
        speed += Random.Range(1, 3);
    }

    #region Æê ½ºÅ³

    protected override void Skill1_1()
    {
        skillDamageValue = atk * (int)(210 * 0.01f);
        speed += speed * 0.1f;
    }

    protected override void Skill1_2()
    {

    }

    protected override void Skill1_3()
    {

    }

    protected override void Skill2_1()
    {

    }

    protected override void Skill2_2()
    {

    }

    protected override void Skill2_3()
    {

    }

    protected override void Skill3_1()
    {

    }

    protected override void Skill3_2()
    {

    }

    protected override void Skill3_3()
    {

    }

    #endregion
}
