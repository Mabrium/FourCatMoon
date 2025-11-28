using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEditor.U2D.Animation;
using UnityEngine;
using static FirebaseSkillExplainMaster;

public class LoadCharacterData : MonoBehaviour
{
    private FirebaseFirestore db;
    private DocumentReference docRef;

    private const string Skill = "Skill";
    [Header("skill String")]
    [SerializeField] private string skillName1 = "null"; //1번 스킬 이름
    [SerializeField] private string skillName2 = "null"; //2번 스킬 이름
    [SerializeField] private string skillName3 = "null"; //3번 스킬 이름

    [SerializeField] private string skillText1 = "null"; //1번 스킬 설명
    [SerializeField] private string skillText2 = "null"; //2번 스킬 설명
    [SerializeField] private string skillText3 = "null"; //3번 스킬 설명

    [Space(8)]
    [SerializeField] private GameObject Cats;
    [SerializeField] private GameObject SkillUI1;
    [SerializeField] private GameObject SkillUI2;
    [SerializeField] private GameObject StatUI;

    [Space(8)]
    [Header("TMP")]
    [SerializeField] private TextMeshProUGUI[] tmp;
    [SerializeField] private TextMeshProUGUI[] statTmp;
    [SerializeField] private TextMeshProUGUI[] skillNameTmp = new TextMeshProUGUI[3]; //스킬 이름 넣는 TMP
    [SerializeField] private TextMeshProUGUI[] skillExplainTmp = new TextMeshProUGUI[3]; //스킬 설명 텍스트 넣는 TMP
    /// <summary>
    /// 스킬 설명 TMP (강화)
    /// </summary>
    [SerializeField] private TextMeshProUGUI[] skillBeefUpTmp = new TextMeshProUGUI[2];
    /// <summary>
    /// 스킬 강화 전 수치
    /// </summary>
    [SerializeField] private TextMeshProUGUI skillBeforeBeefUpTmp;
    /// <summary>
    /// 스킬 강화 후 수치
    /// </summary>
    [SerializeField] private TextMeshProUGUI skillAfterBeefUpTmp;

    [Space(8)]
    [SerializeField] private int catNumberCount;
    [SerializeField] private int catPageNumber = 0;

    [Space(8)]
    [SerializeField] private TextMeshProUGUI allPageNumber;
    [SerializeField] private TextMeshProUGUI nowPageNumber;

    [Space(8)]
    public List<CharacterData> myCharacters = new List<CharacterData>();

    public SkillUI skillUI;

    void Start()
    {
        db = FirebaseFirestore.GetInstance(FirebaseApp.DefaultInstance);
        LoadDataFromFirebase().ContinueWithOnMainThread(task => { TMPChange(); }); //이 부분을 로딩이 끝나면 화면 보이는 방식으로 넣어야 캐릭터가 다 로드 되었을 때에 화면이 전환됨
    }

    private async Task LoadDataFromFirebase()
    {
        int characterCount = 0;

        var snapshot = await db
            .Collection(FirebaseString.PlayerID)
            .Document(Manager.userID)
            .Collection(FirebaseString.CharacterData)
            .GetSnapshotAsync(Source.Server);
        myCharacters.Clear();

        foreach (var charC in snapshot.Documents)
        {
            var localPatName = charC.Id;

            //DataLoad(localPatName);
            var patDataSnapShot = await db
                .Collection(FirebaseString.PlayerID)
                .Document(Manager.userID)
                .Collection(FirebaseString.CharacterData)
                .Document(localPatName)
                .GetSnapshotAsync(Source.Server);

            var Data = patDataSnapShot.ToDictionary();
            characterCount = TUtil.GetValue<int>(Data, localPatName);

            for (int i = 1; i < characterCount + 1; i++)
            {
                Cats = Instantiate(Cats, transform.position, Quaternion.identity);
                CharacterData characterData = Cats.GetComponent<CharacterData>();

                characterData.characterNumber = catNumberCount;

                var snapshot1 = await db
                    .Collection(FirebaseString.PlayerID)
                    .Document(Manager.userID)
                    .Collection(FirebaseString.CharacterData)
                    .Document(localPatName)
                    .Collection(localPatName + i)
                    .Document(localPatName + i + "Data")
                    .GetSnapshotAsync(Source.Server);

                var Data1 = snapshot1.ToDictionary();

                characterData.patName = localPatName;
                characterData.showLevel = TUtil.GetValue<int>(Data1, FirebaseString.LEVEL);
                characterData.showExp = TUtil.GetValue<int>(Data1, FirebaseString.EXP);
                characterData.atk = TUtil.GetValue<int>(Data1, FirebaseString.ATK);
                characterData.def = TUtil.GetValue<int>(Data1, FirebaseString.DEF);
                characterData.maxHp = TUtil.GetValue<int>(Data1, FirebaseString.MAXHP);
                characterData.speed = TUtil.GetValue<int>(Data1, FirebaseString.SPEED);
                characterData.skillPoint = TUtil.GetValue<int>(Data1, FirebaseString.SKILLPOINT);

                var snapshot2 = await db
                    .Collection(FirebaseString.PlayerID)
                    .Document(Manager.userID)
                    .Collection(FirebaseString.CharacterData)
                    .Document(localPatName)
                    .Collection(localPatName + i)
                    .Document(localPatName + i + "Skill")
                    .GetSnapshotAsync(Source.Server);

                var Data2 = snapshot2.ToDictionary();

                characterData.skill1Number = TUtil.GetValue<int>(Data2, FirebaseString.SKILL1NUMBER);
                characterData.skill2Number = TUtil.GetValue<int>(Data2, FirebaseString.SKILL2NUMBER);
                characterData.skill3Number = TUtil.GetValue<int>(Data2, FirebaseString.SKILL3NUMBER);
                characterData.skill1Lv = TUtil.GetValue<int>(Data2, FirebaseString.SKILL1LV);
                characterData.skill2Lv = TUtil.GetValue<int>(Data2, FirebaseString.SKILL2LV);
                characterData.skill3Lv = TUtil.GetValue<int>(Data2, FirebaseString.SKILL3LV);

                myCharacters.Add(characterData);
                catNumberCount++;
            }
        }
        allPageNumber.text = (catNumberCount).ToString();
    }

    private void LoadData()
    {
        db.Collection(FirebaseString.PlayerID).Document(Manager.userID).Collection(FirebaseString.CharacterData)
        .GetSnapshotAsync(Source.Server).ContinueWithOnMainThread(task =>
        {
            var snapshot = task.Result;
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("니꺼 없대");
            }
            myCharacters.Clear();

            foreach (var charC in snapshot.Documents)
            {
                var localPatName = charC.Id;

                //DataLoad(localPatName);
            }
        });
        TMPChange();
    }

    private void DataLoad(string patName)
    {
        int characterCount = 0;
        docRef = db.Collection(FirebaseString.PlayerID).Document(Manager.userID).Collection(FirebaseString.CharacterData).Document(patName);
        docRef.GetSnapshotAsync(Source.Server).ContinueWithOnMainThread(task =>
        {
            var snapshot = task.Result;
            var Data = snapshot.ToDictionary();
            characterCount = TUtil.GetValue<int>(Data, patName);

            for (int i = 1; i < characterCount + 1; i++)
            {
                Cats = Instantiate(Cats, transform.position, Quaternion.identity);
                CharacterData characterData = Cats.GetComponent<CharacterData>();

                characterData.characterNumber = catNumberCount;

                docRef = db.Collection(FirebaseString.PlayerID).Document(Manager.userID).Collection(FirebaseString.CharacterData).Document(patName).Collection(patName + i).Document(patName + i + "Data");
                docRef.GetSnapshotAsync(Source.Server).ContinueWithOnMainThread(task =>
                {
                    var snapshot1 = task.Result;
                    var Data1 = snapshot1.ToDictionary();

                    characterData.patName = patName;
                    characterData.showLevel = TUtil.GetValue<int>(Data1, FirebaseString.LEVEL);
                    characterData.showExp = TUtil.GetValue<int>(Data1, FirebaseString.EXP);
                    characterData.atk = TUtil.GetValue<int>(Data1, FirebaseString.ATK);
                    characterData.def = TUtil.GetValue<int>(Data1, FirebaseString.DEF);
                    characterData.maxHp = TUtil.GetValue<int>(Data1, FirebaseString.MAXHP);
                    characterData.speed = TUtil.GetValue<int>(Data1, FirebaseString.SPEED);
                    characterData.skillPoint = TUtil.GetValue<int>(Data1, FirebaseString.SKILLPOINT);

                });

                docRef = db.Collection(FirebaseString.PlayerID).Document(Manager.userID).Collection(FirebaseString.CharacterData).Document(patName).Collection(patName + i).Document(patName + i + "Skill");
                docRef.GetSnapshotAsync(Source.Server).ContinueWithOnMainThread(task =>
                {
                    var snapshot2 = task.Result;
                    var Data2 = snapshot2.ToDictionary();

                    characterData.skill1Number = TUtil.GetValue<int>(Data2, FirebaseString.SKILL1NUMBER);
                    characterData.skill2Number = TUtil.GetValue<int>(Data2, FirebaseString.SKILL2NUMBER);
                    characterData.skill3Number = TUtil.GetValue<int>(Data2, FirebaseString.SKILL3NUMBER);
                    characterData.skill1Lv = TUtil.GetValue<int>(Data2, FirebaseString.SKILL1LV);
                    characterData.skill2Lv = TUtil.GetValue<int>(Data2, FirebaseString.SKILL2LV);
                    characterData.skill3Lv = TUtil.GetValue<int>(Data2, FirebaseString.SKILL3LV);
                });
                myCharacters.Add(characterData);
                catNumberCount++;
            }
        });
    }



    private void TMPChange()
    {
        PatNameTranslate();
        tmp[0].text = myCharacters[catPageNumber].atk.ToString();
        statTmp[0].text = myCharacters[catPageNumber].atk.ToString();
        tmp[1].text = myCharacters[catPageNumber].def.ToString();
        statTmp[1].text = myCharacters[catPageNumber].def.ToString();
        tmp[2].text = myCharacters[catPageNumber].maxHp.ToString();
        statTmp[2].text = myCharacters[catPageNumber].maxHp.ToString();
        tmp[3].text = myCharacters[catPageNumber].speed.ToString();
        statTmp[3].text = myCharacters[catPageNumber].speed.ToString();
        statTmp[4].text = myCharacters[catPageNumber].showLevel.ToString();
        statTmp[5].text = myCharacters[catPageNumber].skill1Lv.ToString();
        statTmp[6].text = myCharacters[catPageNumber].skill2Lv.ToString();
        statTmp[7].text = myCharacters[catPageNumber].skill3Lv.ToString();
    }

    public void SkillExplainChangeTMP()
    {
        TaskSkillExplainChangeTMP().ContinueWithOnMainThread(task => { ChangeText(); });
    }

    private async Task TaskSkillExplainChangeTMP()
    {
        CharacterData characterData = myCharacters[catPageNumber];
        int i = 1;
        while (i < 4)
        {
            string sSkillNumber = Skill + i;
            string sSkillDashNumber = "null";

            switch (i)
            {
                case 1: sSkillDashNumber = characterData.skill1Number.ToString(); break;
                case 2: sSkillDashNumber = characterData.skill2Number.ToString(); break;
                case 3: sSkillDashNumber = characterData.skill3Number.ToString(); break;
            }
            var snapshot = await db.Collection(FirebaseString.DBCharacterSkill).
            Document(myCharacters[catPageNumber].patName).
            Collection(sSkillNumber).
            Document(sSkillDashNumber).GetSnapshotAsync(Source.Server);

            var Data = snapshot.ToDictionary();

            if (sSkillDashNumber == "0")
            {
                switch (i)
                {
                    case 1: skillName1 = "없음"; skillText1 = "아직 스킬을 배우지 않았습니다"; break;
                    case 2: skillName2 = "없음"; skillText2 = "아직 스킬을 배우지 않았습니다"; break;
                    case 3: skillName3 = "없음"; skillText3 = "아직 스킬을 배우지 않았습니다"; break;
                }
            }
            else
            {
                switch (i)
                {
                    case 1: skillName1 = TUtil.GetValue<string>(Data, FirebaseString.SKILLNAME); skillText1 = TUtil.GetValue<string>(Data, FirebaseString.SKILLEXPLAIN); break;
                    case 2: skillName2 = TUtil.GetValue<string>(Data, FirebaseString.SKILLNAME); skillText2 = TUtil.GetValue<string>(Data, FirebaseString.SKILLEXPLAIN); break;
                    case 3: skillName3 = TUtil.GetValue<string>(Data, FirebaseString.SKILLNAME); skillText3 = TUtil.GetValue<string>(Data, FirebaseString.SKILLEXPLAIN); break;
                }
            }
            i++;
        }
    }

    private void ChangeText()
    {
        skillExplainTmp[0].text = skillText1;
        skillExplainTmp[1].text = skillText2;
        skillExplainTmp[2].text = skillText3;
        skillNameTmp[0].text = skillName1;
        skillNameTmp[1].text = skillName2;
        skillNameTmp[2].text = skillName3;
    }

    public void RightCat()
    {
        if (catPageNumber < catNumberCount - 1)
        {
            catPageNumber++;
            nowPageNumber.text = (catPageNumber + 1).ToString();
            TMPChange();
            SkillExplainChangeTMP();
        }
        else return;
    }

    public void LeftCat()
    {
        if (catPageNumber > 0)
        {
            catPageNumber--;
            nowPageNumber.text = (catPageNumber + 1).ToString();
            TMPChange();
            SkillExplainChangeTMP();
        }
        else return;
    }

    private void PatNameTranslate()
    {
        string pName = "Cat";
        switch (myCharacters[catPageNumber].patName)
        {
            case FirebaseString.BloodMoonCat:
                pName = "적월";
                break;
            case FirebaseString.BlueMoonCat:
                pName = "청월";
                break;
            case FirebaseString.FullMoonCat:
                pName = "만월";
                break;
            case FirebaseString.LunarEclipseCat:
                pName = "월식";
                break;
            case FirebaseString.SBBMoonCat:
                pName = "슈퍼블러드블루문";
                break;
            case FirebaseString.SolarEclipseCat:
                pName = "일식";
                break;
            case FirebaseString.SuperMoonCat:
                pName = "슈퍼문";
                break;
        }
        tmp[4].text = pName;
        statTmp[8].text = pName;
    }
    public void SelectCat()
    {
        Manager.Instance.SetPlayerCat(myCharacters[catPageNumber]);
    }

    public void SkillBeefUp(int number)
    {
        skillUI.characterData = myCharacters[catPageNumber];
        switch (number)
        {
            case 1: skillBeefUpTmp[0].text = skillExplainTmp[0].text; skillBeefUpTmp[1].text = skillExplainTmp[0].text; skillBeforeBeefUpTmp.text = myCharacters[catPageNumber].skill1FigureValue.ToString(); skillAfterBeefUpTmp.text = myCharacters[catPageNumber].skill1FigureValue.ToString(); break; //아직 강화 이전 수치랑 강화 이후 수치 비교를 할 변수가 없음
            case 2: skillBeefUpTmp[0].text = skillExplainTmp[1].text; skillBeefUpTmp[1].text = skillExplainTmp[1].text; skillBeforeBeefUpTmp.text = myCharacters[catPageNumber].skill2FigureValue.ToString(); skillAfterBeefUpTmp.text = myCharacters[catPageNumber].skill2FigureValue.ToString(); break;
            case 3: skillBeefUpTmp[0].text = skillExplainTmp[2].text; skillBeefUpTmp[1].text = skillExplainTmp[2].text; skillBeforeBeefUpTmp.text = myCharacters[catPageNumber].skill3FigureValue.ToString(); skillAfterBeefUpTmp.text = myCharacters[catPageNumber].skill3FigureValue.ToString(); break;
        }
    }


    #region UI 보이기

    public void ShowSkillUI()
    {
        SkillUI1.SetActive(true);
    }

    public void HideSkillUI()
    {
        SkillUI1.SetActive(false);
        SkillUI2.SetActive(false);
    }

    public void ShowStatUI()
    {
        StatUI.SetActive(true);
    }

    public void HideStatUI()
    {
        StatUI.SetActive(false);
    }
    #endregion

}