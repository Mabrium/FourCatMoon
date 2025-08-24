using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public BattleCat[] battleCats = new BattleCat[2];


    void Start()
    {

        TurnManager();
    }

    void Update()
    {
        
    }

    public void TurnManager()
    {
        if (battleCats[0].turnSpeed > battleCats[1].turnSpeed)
        {
            battleCats[0].MyTurnStart();
        }
        else if (battleCats[0].turnSpeed < battleCats[1].turnSpeed)
        {
            battleCats[1].MyTurnStart();
        }
        else if (battleCats[0].turnSpeed == battleCats[1].turnSpeed)
        {
            int rand = Random.Range(0, 2);
            battleCats[rand].MyTurnStart();
        }
    }

    public void SkillNameUpdate()
    {

    }
}
