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

    public string test = "BloodMoonCat";
    public string skill = "Skill1";
    public string skillNumber = "1";

    public string skillName;
    public string skillExplainText;

    private void Start()
    {
        db = FirebaseFirestore.GetInstance(FirebaseApp.DefaultInstance);
    }

    public void UpdateSkillExplain()
    {
        docRef = db.Collection(FirebaseString.DBCharacterSkill).Document(test).Collection(skill).Document(skillNumber);
        Dictionary<string, object> SkillData = new()
        {
            {FirebaseString.SKILLNAME, skillName},
            {FirebaseString.SKILLEXPLAIN, skillExplainText}
        };
        docRef.SetAsync(SkillData).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Error writing Login: " + task.Exception);
            }
        });

        tmp.text = test + " " + skill + " " + skillNumber + 
            "\n" + skillName + " " + skillExplainText;
    }



}
