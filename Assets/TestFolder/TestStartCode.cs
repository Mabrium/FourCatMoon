using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;

public class TestStartCode : MonoBehaviour
{
    public string ID_Master;
    public string PW_Master;


    private FirebaseFirestore db;
    DocumentReference docRef;
    [SerializeField] private Manager manager;

    void Awake()
    {
        db = FirebaseFirestore.GetInstance(FirebaseApp.DefaultInstance);
        string readID;
        string readPW;
        docRef = db.Collection(FirebaseString.PlayerID).Document(ID_Master).Collection(FirebaseString.Profile).Document($"{ID_Master}_Player_IDPW");
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
                    Debug.Log("잘못 되었거나 없습니다");
                    return;
                }
                var Data = snapshot.ToDictionary();
                readID = TUtil.GetValue<string>(Data, FirebaseString.UserID);
                readPW = TUtil.GetValue<string>(Data, FirebaseString.Password);

                if (ID_Master == readID && PW_Master == readPW)
                {
                    Debug.Log("같음");
                    Manager.userID = ID_Master;
                }
                else if (ID_Master != readID || PW_Master != readPW)
                {
                    Debug.Log("다름");
                }
            }
        });
    }
    


}
