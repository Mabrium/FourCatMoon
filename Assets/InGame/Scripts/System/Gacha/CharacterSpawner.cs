using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;

public class CharacterSpawner : MonoBehaviour
{
    private FirebaseFirestore db;
    [SerializeField] private CharacterData characterData;

    void Start()
    {
        db = FirebaseFirestore.GetInstance(FirebaseApp.DefaultInstance);
        StartRandomStatistics();
        SaveStatistics();
    }

    //���߿� ĳ������ �Ӽ�(��)�� ���� �ٸ��� ���� ��ġ�� �ٲٱ�
    public void StartRandomStatistics()
    {
        characterData.atk = Random.Range(3, 6);
        characterData.def = Random.Range(4, 6);
        characterData.maxHp = Random.Range(23, 25);
        characterData.speed = Random.Range(10, 12);
    }

    public void SaveStatistics()
    {
        characterData.DataUpdate();
        FirebaseInt.GACHAINT++;
        DocumentReference docRef = db.Collection(FirebaseString.PlayerID).Document(Manager.userID);
        Dictionary<string, object> gachaInt = new()
        {
            {FirebaseString.GACHAINT, FirebaseInt.GACHAINT}
        }; docRef.SetAsync(gachaInt).ContinueWithOnMainThread(task => { });
    }

}
