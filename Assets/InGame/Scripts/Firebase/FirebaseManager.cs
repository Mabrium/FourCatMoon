using UnityEngine;
using Firebase;
using Firebase.Extensions;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance { get; private set; }
    public bool IsInitialized { get; private set; } = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                IsInitialized = true;
                Debug.Log("Firebase �ʱ�ȭ �Ϸ�");
            }
            else
            {
                Debug.LogError("Firebase �ʱ�ȭ ����: " + task.Result);
            }
        });
    }
}
