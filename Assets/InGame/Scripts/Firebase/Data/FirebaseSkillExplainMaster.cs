using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;

public class FirebaseSkillExplainMaster : MonoBehaviour
{
    private FirebaseFirestore db;
    private DocumentReference docRef;

    public string skillExplainText;

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

    void Start()
    {
        db = FirebaseFirestore.GetInstance(FirebaseApp.DefaultInstance);
        docRef = db.Collection(FirebaseString.DBCharacterSkill).Document(catType.ToString()).Collection(FirebaseString.DBCharacterSkillExplain).Document(skillNumber.ToString());
        Dictionary<string, object> SkillData = new()
        {
            {FirebaseString.SKILL1EXPLAIN, skillExplainText }
        };
        docRef.SetAsync(SkillData).ContinueWithOnMainThread(task =>{ });
        Debug.Log(FirebaseString.DBCharacterSkill + "\n" + catType.ToString() + "\n" + FirebaseString.DBCharacterSkillExplain + "\n" + skillNumber.ToString());
        Debug.Log(skillExplainText);
    }



}
