using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleStat : MonoBehaviour
{
    public float turnSpeed;

    public CharacterData charData;
    public TextMeshProUGUI[] tmpUGUI;
    void Start()
    {
        StatUpdata();
        turnSpeed = charData.speed;
    }

    public void MyTurnStart()
    {
        StatUpdata();
    }

    private void StatUpdata()
    {
        tmpUGUI[0].text = charData.atk.ToString();
        tmpUGUI[1].text = charData.def.ToString();
        tmpUGUI[2].text = charData.hp.ToString();
        tmpUGUI[3].text = charData.speed.ToString();
    }
}
