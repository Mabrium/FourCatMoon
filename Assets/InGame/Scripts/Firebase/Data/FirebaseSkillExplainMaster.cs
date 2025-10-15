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
        SBBMoonCat
    }

    public enum SkillNumber
    {
        Skill1,
        Skill2,
        Skill3
    }


    public CatType catType;
    public SkillNumber skillNumber;

    public string skillExplainText;

    public void UpdateSkillExplain()
    {
        db = FirebaseFirestore.GetInstance(FirebaseApp.DefaultInstance);
        docRef = db.Collection(FirebaseString.DBCharacterSkill).Document(catType.ToString()).Collection(FirebaseString.DBCharacterSkillExplain).Document(skillNumber.ToString());
        Dictionary<string, object> SkillData = new()
        {
            {FirebaseString.SKILL1EXPLAIN, skillExplainText }
        };
        docRef.SetAsync(SkillData).ContinueWithOnMainThread(task => { });
        Debug.Log(catType.ToString() + "\n" + skillNumber.ToString());
        Debug.Log(skillExplainText);
        
        tmp.text = (catType.ToString() + "\n" + skillNumber.ToString() + "\n" + skillExplainText);
    }



}
