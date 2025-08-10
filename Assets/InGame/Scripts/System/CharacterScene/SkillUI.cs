using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUI : MonoBehaviour
{
    [SerializeField] private GameObject ReinforceUI;

    public void ShowUI()
    {
        ReinforceUI.SetActive(true);
    }

    public void UpgradeSkill()
    {
        ReinforceUI.SetActive(false);
        //강화 파티클 연출
        //강화된 스킬 계수 보여주기

    }

    public void CancelSkill()
    {
        ReinforceUI.SetActive(false);
    }
}
