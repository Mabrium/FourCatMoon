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

    public string userID;
    public string password;
    private string mainScene = "MainScene";

    public int RememberYesNo;

    void Start()
    {
        db = FirebaseFirestore.GetInstance(FirebaseApp.DefaultInstance);
        RememberYesNo = PlayerPrefs.GetInt("YesNo", RememberYesNo);
        if(RememberYesNo == 1)
        {
            inputField[0].text = PlayerPrefs.GetString("ID");
            inputField[1].text = PlayerPrefs.GetString ("PW");
        }
    }

    private void UserSignUp()
    {
        DocumentReference docRef = db.Collection($"{FirebaseString.PlayerID}").Document(userID).Collection($"{FirebaseString.Profile}").Document($"{userID}_Player_IDPW");
        Dictionary<string, object> UserData = new()
        {
            { FirebaseString.UserID, userID },
            { FirebaseString.Password, password }
        };
        docRef.SetAsync(UserData).ContinueWithOnMainThread(task => {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Error writing Login: " + task.Exception);
            }
        });
    }

    public void UserLogin()
    {
        IDPW();
        string readID;
        string readPW;
        DocumentReference docRef = db.Collection($"{FirebaseString.PlayerID}").Document(userID).Collection($"{FirebaseString.Profile}").Document($"{userID}_Player_IDPW");
        docRef.GetSnapshotAsync(Source.Server).ContinueWithOnMainThread(task => {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Error getting Login: " + task.Exception);
            }
            else
            {
                var snapshot = task.Result;
                if (!snapshot.Exists)
                {
                    Debug.Log("�߸� �Ǿ��ų� �����ϴ�");
                    return;
                }
                var Data = snapshot.ToDictionary();
                readID = GetValue<string>(Data, FirebaseString.UserID);
                readPW = GetValue<string>(Data, FirebaseString.Password);

                if (userID == readID && password == readPW)
                {
                    Debug.Log("����");
                    LoadID();
                    SceneManager.LoadScene(mainScene);
                }
                else if(userID != readID || password != readPW)
                {
                    Debug.Log("�ٸ�");
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
        string readID;
        IDPW();
        DocumentReference docRef = db.Collection($"{FirebaseString.PlayerID}").Document(userID).Collection($"{FirebaseString.Profile}").Document($"{userID}_Player_IDPW");
        docRef.GetSnapshotAsync(Source.Server).ContinueWithOnMainThread(task => {
            var snapshot = task.Result;
            if (!snapshot.Exists)
            {
                UserSignUp();
            }
            var Data = snapshot.ToDictionary();
            readID = GetValue<string>(Data, FirebaseString.UserID);
            if (userID == readID)
            {
                Debug.Log("�ȵ�");
            }
        });

    }

    private void LoadID()
    {
        manager.userID = userID;
    }



    T GetValue<T>(Dictionary<string, object> data, string key)
    {
        if (data.ContainsKey(key))
        {
            try
            {
                return (T)Convert.ChangeType(data[key], typeof(T)); // Ÿ�Կ� �°� ��ȯ
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error converting {key}: {ex.Message}");
            }
        }
        else
        {
            // Ű�� ���� ��� ��� �޽��� ���
            Debug.LogWarning($"Key {key} not found in Firestore data.");
        }
        return default(T); // �⺻�� ��ȯ (���� ���ų� ��ȯ ���� ��)
    }
}
