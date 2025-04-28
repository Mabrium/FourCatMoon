using UnityEngine;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Collections.Generic;
using System;

public class FirebaseUpdateData : MonoBehaviour
{
    private FirebaseFirestore db;

    // ĳ���� �Ӽ�
    public int atk = 50;
    public int def = 30;
    public int hp = 100;
    public float speed = 5.5f;
    public int speedValue = 10;

    void Start()
    {
        InitializeFirebase();
    }

    // Firebase �ʱ�ȭ
    void InitializeFirebase()
    {
        db = FirebaseFirestore.GetInstance(FirebaseApp.DefaultInstance);
        TestFirestore();
    }

    // Firestore�� ������ ���� �� �б� �׽�Ʈ
    void TestFirestore()
    {
        // ���÷� playerId 1, characterId 2�� ���
        int playerId = 1;
        int characterId = 2;

        //WriteCharacterData(playerId, characterId);
        ReadCharacterData(playerId, characterId);
    }

    // ĳ���� ������ Firestore�� ����
    void WriteCharacterData(int playerId, int characterId)
    {
        // playerId�� ���缭 "players" �÷����� �����ϰ� �� �Ʒ� "characters" �÷��ǿ� "character_{characterId}" ������ �߰�
        DocumentReference docRef = db.Collection("players").Document($"player_{playerId}")
                                       .Collection("characters").Document($"character_{characterId}");

        Dictionary<string, object> character = new()
        {
            { "atk", atk },
            { "def", def },
            { "hp", hp },
            { "speed", speed },
            { "speedValue", speedValue }
        };

        docRef.SetAsync(character).ContinueWithOnMainThread(task => {
            if (task.IsFaulted || task.IsCanceled)
            {
                // ������ �߻����� ���� ���
                Debug.LogError("Error writing character data: " + task.Exception);
            }
        });
    }

    // Firestore���� ĳ���� ������ �б�
    void ReadCharacterData(int playerId, int characterId)
    {
        // playerId�� ���缭 "players" �÷����� �����ϰ� �� �Ʒ� "characters" �÷��ǿ� "character_{characterId}" ������ ��ȸ
        DocumentReference docRef = db.Collection("players").Document($"player_{playerId}")
                                       .Collection("characters").Document($"character_{characterId}");

        // ������ �������� �����͸� �о����
        docRef.GetSnapshotAsync(Source.Server).ContinueWithOnMainThread(task => {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Error getting character data: " + task.Exception);
            }
            else
            {
                var snapshot = task.Result;
                var characterData = snapshot.ToDictionary();

                // ȿ������ ������ �б� �� Ÿ�� ��ȯ
                atk = GetValue<int>(characterData, "atk");
                def = GetValue<int>(characterData, "def");
                hp = GetValue<int>(characterData, "hp");
                speed = GetValue<float>(characterData, "speed");
                speedValue = GetValue<int>(characterData, "speedValue");
            }
        });
    }

    // ���׸� �޼ҵ�� �����͸� �а�, Ÿ�Կ� �°� ��ȯ
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
