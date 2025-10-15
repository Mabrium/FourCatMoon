using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public BattleCat[] battleCats = new BattleCat[2];

    [SerializeField] private TextMeshProUGUI[] cat0StatTMP = new TextMeshProUGUI[4];
    [SerializeField] private TextMeshProUGUI[] cat1StatTMP = new TextMeshProUGUI[4];


    void Start()
    {

        StartTurnManager();
    }

    public void StartTurnManager()
    {
        if (battleCats[0].battleCatTurnSpeed > battleCats[1].battleCatTurnSpeed)
        {
            battleCats[0].MyTurnStart();
        }
        else if (battleCats[0].battleCatTurnSpeed < battleCats[1].battleCatTurnSpeed)
        {
            battleCats[1].MyTurnStart();
        }
        else if (battleCats[0].battleCatTurnSpeed == battleCats[1].battleCatTurnSpeed)
        {
            int rand = Random.Range(0, 2);
            battleCats[rand].MyTurnStart();
        }
    }

    public void SkillNameUpdate()
    {

    }
}
