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
    private DocumentReference doRef;
    //[SerializeField] private UsingCat characterDataSTO;
    //[SerializeField] private SkillText skillTextSTO;
    [SerializeField] private GameObject testVoid;

    [SerializeField] private TextMeshProUGUI[] tmp;
    //[SerializeField] private string patName;
    public List<CharacterData> myCharacters = new List<CharacterData>();

    [SerializeField] private int CatNumberCount;


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
        db.Collection(FirebaseString.PlayerID).Document(Manager.userID).Collection(FirebaseString.CharacterData).
        //Document(characterDataSTO.NAME).Collection(characterDataSTO.NAME + characterDataSTO.NUMBER).Document(characterDataSTO.NAME + characterDataSTO.NUMBER + "Data");
        GetSnapshotAsync(Source.Server).ContinueWithOnMainThread(task =>
        {
            var snapshot = task.Result;
            myCharacters.Clear();

            foreach (var charC in snapshot.Documents)
            {
                var Datas = charC.ToDictionary();

                Debug.Log(charC.Id);
                db.Collection(FirebaseString.PlayerID).Document(Manager.userID).Collection(FirebaseString.CharacterData).Document(charC.Id).
                GetSnapshotAsync(Source.Server).ContinueWithOnMainThread(task =>
                {

                    foreach (var downCharC in snapshot.Documents)
                    {
                        var Data = downCharC.ToDictionary();
                        testVoid = Instantiate(testVoid, transform.position, Quaternion.identity);
                        CharacterData characterData = testVoid.GetComponent<CharacterData>();

                        characterData.patName = TUtil.GetValue<string>(Data, downCharC.Id);
                        characterData.showLevel = TUtil.GetValue<int>(Data, FirebaseString.LEVEL);
                        characterData.showExp = TUtil.GetValue<int>(Data, FirebaseString.EXP);
                        characterData.atk = TUtil.GetValue<int>(Data, FirebaseString.ATK);
                        characterData.def = TUtil.GetValue<int>(Data, FirebaseString.DEF);
                        characterData.maxHp = TUtil.GetValue<int>(Data, FirebaseString.MAXHP);
                        characterData.speed = TUtil.GetValue<int>(Data, FirebaseString.SPEED);
                        characterData.skillPoint = TUtil.GetValue<int>(Data, FirebaseString.SKILLPOINT);
                        characterData.skill1Lv = TUtil.GetValue<int>(Data, FirebaseString.SKILL1LV);
                        characterData.skill2Lv = TUtil.GetValue<int>(Data, FirebaseString.SKILL2LV);
                        characterData.skill3Lv = TUtil.GetValue<int>(Data, FirebaseString.SKILL3LV);
                        characterData.skill1Number = TUtil.GetValue<int>(Data, FirebaseString.SKILL1NUMBER);
                        characterData.skill2Number = TUtil.GetValue<int>(Data, FirebaseString.SKILL2NUMBER);
                        characterData.skill3Number = TUtil.GetValue<int>(Data, FirebaseString.SKILL3NUMBER);

                        myCharacters.Add(characterData);
                    }

                });
            }
        });

    }


    public void RightCat()
    {
        if (CatNumberCount != 0)
        {
            CatNumberCount++;
            TMPChange();
        }
        else return;
    }

    public void LeftCat()
    {
        if (CatNumberCount == myCharacters.Count)
        {
            CatNumberCount--;
            TMPChange();
        }
        else return;
    }

    private void TMPChange()
    {
        tmp[0].text = myCharacters[CatNumberCount].atk.ToString();
        tmp[1].text = myCharacters[CatNumberCount].def.ToString();
        tmp[2].text = myCharacters[CatNumberCount].maxHp.ToString();
        tmp[3].text = myCharacters[CatNumberCount].speed.ToString();
    }
}
