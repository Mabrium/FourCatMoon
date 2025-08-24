using UnityEngine;

using System;
using System.Collections.Generic;

using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;

public class CharacterData : MonoBehaviour
{
    [Header("NUMBER")]
    public int characterNumber;

    [Header("NAME")]
    public string patName;

    [Space(4f)]
    private FirebaseFirestore db;
    DocumentReference docRef;

    [Header("Statistics")]
    [SerializeField] private int level = 1;
    public int showLevel
    {
        get => level;
        set => level = value;
    }
    [SerializeField] private int exp;
    public int showExp
    {
        get => exp;
        set => exp = value;
    }
    public int atk;
    public int def;
    public int maxHp;
    public float speed;
    private int speedValue;
    public int skillPoint;
    public int skill1Number;
    public int skill2Number;
    public int skill3Number;
    public int skill1Lv;
    public int skill2Lv;
    public int skill3Lv;

    private int expI = 0;
    private int spLvI = 0;
    private int[] skillPointLevel = { 5, 10, 30 };
    private int[] upExp = { 10, 20, 40, 80, 140, 220, 320, 450, 500, 510, 520, 530, 540, 550, 560, 570, 580, 590, 600, 610, 620, 630, 640, 650, 660, 670, 680, 690, 700 };

    

    public void Leveling(int expValue)
    {
        exp += expValue;
        if (exp >= upExp[expI])
        {
            level++;
            StatisticsUp();
            exp = exp - upExp[expI];
            expI++;
            if (level == skillPointLevel[spLvI])
            {
                skillPoint++;
                spLvI++;
            }
            if(level == 20)
            {
                //3레벨 스킬 배우기
            }
        }
        if (level == 30)
        {
            //만렙 달성시 되는 것
        }
    }

    protected void Skill1()
    {
        switch (skill1Number)
        {
            case 0: break;//없음
            case 1: Skill1_1(); break;
            case 2: Skill1_2(); break;
            case 3: Skill1_3(); break;
        }
    }

    protected void Skill2()
    {
        switch (skill2Number)
        {
            case 0: break;
            case 1: Skill2_1(); break;
            case 2: Skill2_2(); break;
            case 3: Skill2_3(); break;
        }
    }

    protected void Skill3()
    {
        switch (skill3Number)
        {
            case 0: break;
            case 1: Skill3_1(); break;
            case 2: Skill3_2(); break;
            case 3: Skill3_3(); break;
        }
    }

    //public void 

    protected virtual void StatisticsUp()
    {

    }

    /// <summary>
    /// 속도값 1, 0.5, 2 나오는것
    /// </summary>
    protected void Speedvalue()
    {
        speedValue = UnityEngine.Random.Range(0, 3);
        switch (speedValue)
        {
            case 0: speed += 0;
                break;
            case 1: speed += (float)0.5;
                break;
            case 2: speed += 1;
                break;
        }
    }

    #region 세부 스킬
    protected virtual void Skill1_1()
    {

    }

    protected virtual void Skill1_2()
    {

    }

    protected virtual void Skill1_3()
    {

    }

    protected virtual void Skill2_1()
    {

    }

    protected virtual void Skill2_2()
    {

    }

    protected virtual void Skill2_3()
    {

    }

    protected virtual void Skill3_1()
    {

    }

    protected virtual void Skill3_2()
    {

    }

    protected virtual void Skill3_3()
    {

    }

    #endregion

    public void DataUpdate()
    {
        db = FirebaseFirestore.GetInstance(FirebaseApp.DefaultInstance);
        docRef = db.Collection(FirebaseString.PlayerID).Document(Manager.userID).Collection(FirebaseString.CharacterData).Document(patName).Collection(patName + characterNumber).Document(patName + characterNumber + "Data");
        Dictionary<string, object> characterData = new()
        {
            {FirebaseString.LEVEL, level},
            {FirebaseString.EXP, exp},
            {FirebaseString.SKILLPOINT, skillPoint},
            {FirebaseString.ATK, atk},
            {FirebaseString.DEF, def},
            {FirebaseString.MAXHP, maxHp},
            {FirebaseString.SPEED, speed}
        };
        docRef.SetAsync(characterData).ContinueWithOnMainThread(task => { });
        docRef = db.Collection(FirebaseString.PlayerID).Document(Manager.userID).Collection(FirebaseString.CharacterData).Document(patName).Collection(patName + characterNumber).Document(patName + characterNumber + "Skill");
        Dictionary<string, object> characterSkill = new()
        {
            {FirebaseString.SKILL1NUMBER, skill1Number},
            {FirebaseString.SKILL2NUMBER, skill2Number},
            {FirebaseString.SKILL3NUMBER, skill3Number}
        };
        docRef.SetAsync(characterSkill).ContinueWithOnMainThread(task => { });
    }

    public void DataLoad()
    {
        db = FirebaseFirestore.GetInstance(FirebaseApp.DefaultInstance);
        docRef = db.Collection(FirebaseString.PlayerID).Document(Manager.userID).Collection(FirebaseString.CharacterData).Document(patName).Collection(patName + characterNumber).Document(patName + characterNumber + "Data");
        docRef.GetSnapshotAsync(Source.Server).ContinueWithOnMainThread(task =>
        {
            var snapshot = task.Result;
            var Data = snapshot.ToDictionary();
            level = TUtil.GetValue<int>(Data, FirebaseString.LEVEL);
            exp = TUtil.GetValue<int>(Data, FirebaseString.EXP);
            skillPoint = TUtil.GetValue<int>(Data, FirebaseString.SKILLPOINT);
            atk = TUtil.GetValue<int>(Data, FirebaseString.ATK);
            def = TUtil.GetValue<int> (Data, FirebaseString.DEF);
            maxHp = TUtil.GetValue<int>(Data, FirebaseString.MAXHP);
            speed = TUtil.GetValue<int>(Data, FirebaseString.SPEED);
        });
        docRef = db.Collection(FirebaseString.PlayerID).Document(Manager.userID).Collection(FirebaseString.CharacterData).Document(patName).Collection(patName + characterNumber).Document(patName + characterNumber + "Skill");
        docRef.GetSnapshotAsync(Source.Server).ContinueWithOnMainThread(task =>
        {
            var snapshot = task.Result;
            var Data = snapshot.ToDictionary();
            skill1Number = TUtil.GetValue<int>(Data, FirebaseString.SKILL1NUMBER);
            skill2Number = TUtil.GetValue<int>(Data, FirebaseString.SKILL2NUMBER);
            skill3Number = TUtil.GetValue<int>(Data, FirebaseString.SKILL3NUMBER);
        });
    }
}
