using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadCharacterData : MonoBehaviour
{
    private FirebaseFirestore db;
    private DocumentReference docRef;
    //[SerializeField] private UsingCat characterDataSTO;
    //[SerializeField] private SkillText skillTextSTO;
    [SerializeField] private GameObject Cats;
    [SerializeField] private GameObject SkillUI;
    [SerializeField] private GameObject StatUI;

    [SerializeField] private TextMeshProUGUI[] tmp;
    //[SerializeField] private string patName;
    public List<CharacterData> myCharacters = new List<CharacterData>();

    [SerializeField] private int catNumberCount;
    [SerializeField] private int catPageNumber = 0;

    void Start()
    {
        db = FirebaseFirestore.GetInstance(FirebaseApp.DefaultInstance);
        LoadData();
    }

    void Update()
    {

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

                DataLoad(localPatName);
                //Debug.Log(localPatName);
            }
        });
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
                //여기에 만약 다 불러왔다면 TMPChange시키는거 만들기
            }
        });
    }

    public void RightCat()
    {
        if (catPageNumber < catNumberCount - 1)
        {
            catPageNumber++;
            TMPChange();
        }
        else return;
    }

    public void LeftCat()
    {
        if (catPageNumber > 0)
        {
            catPageNumber--;
            TMPChange();
        }
        else return;
    }


    private void TMPChange()
    {
        Debug.Log("불러옴");
        tmp[0].text = myCharacters[catPageNumber].atk.ToString();
        Debug.Log(tmp[0].text);
        tmp[1].text = myCharacters[catPageNumber].def.ToString();
        Debug.Log(tmp[1].text);
        tmp[2].text = myCharacters[catPageNumber].maxHp.ToString();
        Debug.Log(tmp[2].text);
        tmp[3].text = myCharacters[catPageNumber].speed.ToString();
        Debug.Log(tmp[3].text);
        PatNameTranslate();
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
    }


    #region UI 보이기

    public void ShowSkillUI()
    {
        SkillUI.SetActive(true);
    }

    public void HideSkillUI()
    {
        SkillUI.SetActive(false);
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