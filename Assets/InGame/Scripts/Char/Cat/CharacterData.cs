using UnityEngine;

using System.Collections.Generic;

using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;

public class CharacterData : MonoBehaviour
{
    private FirebaseFirestore db;
    DocumentReference docRef;

    [Header("NUMBER")]
    public int characterNumber;

    [Header("NAME")]
    public string patName;

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

    //스킬 강화했을 때 배열에 알맞게 맞춰줘서 값을 얻을 int
    public int skill1FigureI = 0;
    public int skill2FigureI = 0;
    public int skill3FigureI = 0;
    //스킬 강화하면 늘어날 계수들을 넣는 배열
    protected int[] skill1Figure;
    protected int[] skill2Figure;
    protected int[] skill3Figure;
    //스킬 강화할 때에 바뀔 값
    public int skill1FigureValue;
    public int skill2FigureValue;
    public int skill3FigureValue;

    public int damage;
    public int skillDamageValue;

    //아직 레벨링을 했을 때 레벨이 같으면 중복으로 작동되는걸 안 막았음
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
                skill3Lv = 1;
                skill3Number = Random.Range(1, 4);
            }
        }
        if (level == 30)
        {
            //만렙 달성시 되는 것
        }
    }

    public void Skill1()
    {
        switch (skill1Number)
        {
            case 0: break; //없음
            case 1: Skill1_1(); break;
            case 2: Skill1_2(); break;
            case 3: Skill1_3(); break;
        }
    }

    public void Skill2()
    {
        switch (skill2Number)
        {
            case 0: break;
            case 1: Skill2_1(); break;
            case 2: Skill2_2(); break;
            case 3: Skill2_3(); break;
        }
    }

    public void Skill3()
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
            {FirebaseString.SKILL3NUMBER, skill3Number},
            {FirebaseString.SKILL1LV, skill1Lv},
            {FirebaseString.SKILL2LV, skill2Lv},
            {FirebaseString.SKILL3LV, skill3Lv}
        };
        docRef.SetAsync(characterSkill).ContinueWithOnMainThread(task => { });
    }

}
