using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUI : MonoBehaviour
{
    [SerializeField] private GameObject ReinforceUI;
    [SerializeField] private LoadCharacterData loadCharacterData;
    public CharacterData characterData;

    public void ShowUI(int skillNumber)
    {
        ReinforceUI.SetActive(true);
        loadCharacterData.SkillBeefUp(skillNumber);
    }

    public void UpgradeSkill(int skillNumber)
    {
        ReinforceUI.SetActive(false);
        switch (skillNumber)
        {
            case 1: characterData.skill1FigureI += 1; break;
            case 2: characterData.skill2FigureI += 1; break;
            case 3: characterData.skill3FigureI += 1; break;
        }
        //강화 파티클 연출
        //강화된 스킬 계수 보여주기

    }

    public void CancelSkill()
    {
        ReinforceUI.SetActive(false);
    }
}
