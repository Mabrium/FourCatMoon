using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using TMPro;

public class FirebaseSkillExplainMaster : MonoBehaviour
{
    private FirebaseFirestore db;
    private DocumentReference docRef;

    [SerializeField] private TextMeshProUGUI tmp;

    public enum CatType
    {
        FirstQuarterCat,
        NewMoonCat,
        OldMoonCat,
        ThirdQuarterCat,
        WCMoonCat,
        WGMoonCat,
        BloodMoonCat,
        BlueMoonCat,
        FullMoonCat,
        LunarEclipseCat,
        SolarEclipseCat,
        SuperMoonCat,
        SBBMoonCat,
    }

    public enum SkillNumber
    {
        Skill1,
        Skill2,
        Skill3,
    }



    public CatType catType;
    public SkillNumber skillNumber;
    [Range(1, 3)]
    public int Skill_Number;
    [Space(10)]
    public string skillExplainText;

    public void UpdateSkillExplain()
    {
        db = FirebaseFirestore.GetInstance(FirebaseApp.DefaultInstance);
        docRef = db.Collection(FirebaseString.DBCharacterSkill).Document(catType.ToString()).Collection(skillNumber.ToString()).Document(Skill_Number.ToString());
        Dictionary<string, object> SkillData = new()
        {
            {FirebaseString.SKILLEXPLAIN, skillExplainText}
        };
        docRef.SetAsync(SkillData).ContinueWithOnMainThread(task => { });
        Debug.Log(catType.ToString() + "\n" + skillNumber.ToString() + Skill_Number);
        Debug.Log(skillExplainText);
        
        tmp.text = (catType.ToString() + "\n" + skillNumber.ToString() + Skill_Number + "\n" + skillExplainText);
    }



}
