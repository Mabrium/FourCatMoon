using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class LoadCharacterData : MonoBehaviour
{
    private FirebaseFirestore db;
    private DocumentReference docRef;
    //[SerializeField] private UsingCat characterDataSTO;
    //[SerializeField] private SkillText skillTextSTO;
    [SerializeField] private GameObject testVoid;

    [SerializeField] private TextMeshProUGUI[] tmp;
    //[SerializeField] private string patName;
    public List<CharacterData> myCharacters = new List<CharacterData>();

    [SerializeField] private int catNumberCount;
    [SerializeField] private int catPageNumber = 1;

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
                Debug.LogError("´Ï²¨ ¾ø´ë");
            }
            myCharacters.Clear();

            foreach (var charC in snapshot.Documents)
            {
                var localPatName = charC.Id;
                
                DataLoad(localPatName);
                Debug.Log(localPatName);
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
                testVoid = Instantiate(testVoid, transform.position, Quaternion.identity);
                CharacterData characterData = testVoid.GetComponent<CharacterData>();
                
                characterData.characterNumber = catNumberCount;
                ;

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

    public void RightCat()
    {
        if (catPageNumber < catNumberCount)
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
        tmp[0].text = myCharacters[catPageNumber].atk.ToString();
        tmp[1].text = myCharacters[catPageNumber].def.ToString();
        tmp[2].text = myCharacters[catPageNumber].maxHp.ToString();
        tmp[3].text = myCharacters[catPageNumber].speed.ToString();
    }
}