using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

using System;
using System.Collections.Generic;

using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;


public class Login : MonoBehaviour
{
    private FirebaseFirestore db;
    [SerializeField] private Manager manager;
    [SerializeField] private TMP_InputField[] inputField;
    DocumentReference docRef;

    public string userID;
    public string password;
    private string mainScene = "MainScene";

    public int RememberYesNo;

    void Start()
    {
        db = FirebaseFirestore.GetInstance(FirebaseApp.DefaultInstance);
        RememberYesNo = PlayerPrefs.GetInt("YesNo", RememberYesNo);
        if (RememberYesNo == 1)
        {
            inputField[0].text = PlayerPrefs.GetString("ID");
            inputField[1].text = PlayerPrefs.GetString("PW");
        }
    }

    private void UserSignUp()
    {
        if (!manager.internetCheck)
        {
            Debug.Log("인터넷 연결을 확인해주세요");
        }
        else
        {
            docRef = db.Collection(FirebaseString.PlayerID).Document(userID).Collection(FirebaseString.Profile).Document($"{userID}_Player_IDPW");
            Dictionary<string, object> UserData = new()
            {
                { FirebaseString.UserID, userID },
                { FirebaseString.Password, password }
            };
            docRef.SetAsync(UserData).ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    Debug.LogError("Error writing Login: " + task.Exception);
                }
            });
        }
    }

    public void UserLogin()
    {
        if (!manager.internetCheck)
        {
            Debug.Log("인터넷 연결을 확인해주세요");
            return;
        }
        IDPW();
        string readID;
        string readPW;
        docRef = db.Collection(FirebaseString.PlayerID).Document(userID).Collection(FirebaseString.Profile).Document($"{userID}_Player_IDPW");
        docRef.GetSnapshotAsync(Source.Server).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Error getting Login: " + task.Exception);
            }
            else
            {
                var snapshot = task.Result;

                if (!snapshot.Exists)
                {
                    Debug.Log("잘못 되었거나 없습니다");
                    return;
                }


                var Data = snapshot.ToDictionary();
                readID = TUtil.GetValue<string>(Data, FirebaseString.UserID);
                readPW = TUtil.GetValue<string>(Data, FirebaseString.Password);

                if (userID == readID && password == readPW)
                {
                    Debug.Log("같음");
                    LoadID();
                    SceneManager.LoadScene(mainScene);
                }
                else if (userID != readID || password != readPW)
                {
                    Debug.Log("다름");
                    return;
                }
            }
        });
    }

    public void IDPW()
    {
        userID = inputField[0].GetComponent<TMP_InputField>().text;
        password = inputField[1].GetComponent<TMP_InputField>().text;
    }

    public void IDCheck()
    {
        if (!manager.internetCheck)
        {
            Debug.Log("인터넷 연결을 확인해주세요");
            return;
        }
        string readID;
        IDPW();
        docRef = db.Collection(FirebaseString.PlayerID).Document(userID).Collection(FirebaseString.Profile).Document($"{userID}_Player_IDPW");
        docRef.GetSnapshotAsync(Source.Server).ContinueWithOnMainThread(task =>
        {
            var snapshot = task.Result;
            if (!snapshot.Exists)
            {
                UserSignUp();
            }
            var Data = snapshot.ToDictionary();
            readID = TUtil.GetValue<string>(Data, FirebaseString.UserID);
            if (userID == readID)
            {
                Debug.Log("안돼");
            }
        });

    }

    private void LoadID()
    {
        Manager.userID = userID;
    }

}
