using UnityEngine;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Collections.Generic;
using System;
using TMPro;


public class Login : MonoBehaviour
{
    private FirebaseFirestore db;
    [SerializeField] private TMP_InputField[] inputField;
    public string UserID;
    public string Password;

    private string ID;
    private string PW;

    void Start()
    {
        db = FirebaseFirestore.GetInstance(FirebaseApp.DefaultInstance);
    }

    public void UserSignUp()
    {
        IDPW();
        print(ID);
        print(PW);
        DocumentReference docRef = db.Collection("PlayerID").Document($"{ID}").Collection("Password").Document($"{PW}");
        Dictionary<string, object> UserData = new()
        {
            { "UserID", UserID},
            { "Password", Password}
        };
        docRef.SetAsync(UserData).ContinueWithOnMainThread(task => {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Error writing character data: " + task.Exception);
            }
        });
    }

    public void UserLogin()
    {
        IDPW();
        print(ID);
        print(PW);
        string readID;
        string readPW;
        DocumentReference docRef = db.Collection("PlayerID").Document($"{ID}").Collection("Password").Document($"{PW}");
        docRef.GetSnapshotAsync(Source.Server).ContinueWithOnMainThread(task => {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Error getting character data: " + task.Exception);
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
                readID = GetValue<string>(Data, "UserID");
                readPW = GetValue<string>(Data, "Password");


                if (UserID == readID && Password == readPW)
                {
                    Debug.Log("����");
                }
            }
        });
    }

    private void IDPW()
    {
        UserID = inputField[0].GetComponent<TMP_InputField>().text;
        Password = inputField[1].GetComponent<TMP_InputField>().text;
        ID = UserID;
        PW = Password;
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
