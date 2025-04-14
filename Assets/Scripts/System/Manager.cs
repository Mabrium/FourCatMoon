using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public static Manager Instance;

    private Stack<string> sceneHistory = new Stack<string>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �� �ı� ����
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }


    #region ��
    /// <summary>
    /// �ٸ� ������ �Ѿ�µ� �� ȭ������ ���ư��Ⱑ �ʿ��ϸ� ���
    /// </summary>
    public void LoadScene(string sceneName)
    {
        // ���� �� �̸� ����
        string currentScene = SceneManager.GetActiveScene().name;
        sceneHistory.Push(currentScene);

        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// ���� ������ ���� ������ ���ư���
    /// </summary>
    public void GoBack()
    {
        if (sceneHistory.Count > 0)
        {
            string previousScene = sceneHistory.Pop();
            SceneManager.LoadScene(previousScene);
        }
        if (sceneHistory.Count == 0)
        {
            //�����Ѱų� ���� UI ���� OK ������ �Ʒ� �ڵ�� �ݱ�
            //Application.Quit();
        }
    }
    #endregion
}
