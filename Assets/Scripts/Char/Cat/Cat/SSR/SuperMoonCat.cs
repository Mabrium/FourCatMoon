using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperMoonCat : CharacterData
{
    //���۹�
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    protected override void StatisticsUp()
    {
        atk += Random.Range(2, 5);
        def += Random.Range(7, 9);
        hp += Random.Range(26, 29);
        speed += Random.Range(1, 3);
    }
}
