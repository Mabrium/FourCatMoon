using BackEnd.Quobject.SocketIoClientDotNet.Client;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public static Manager Instance;

    public static string userID;
    public bool internetCheck;
    [SerializeField] private bool firstInternetCheck;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI internetTMP;

    private NetworkReachability prevReachability;

    private Stack<string> sceneHistory = new Stack<string>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        var currentReachability = Application.internetReachability;

        if (currentReachability != prevReachability)
        {
            internetCheck = currentReachability != NetworkReachability.NotReachable;
            OnInternetStatusChanged(internetCheck);
            prevReachability = currentReachability;
        }
    }

    private void OnInternetStatusChanged(bool isConnected)
    {
        if (!isConnected)
        {
            if (SceneManager.GetActiveScene().buildIndex != 0)
            {
                SceneManager.LoadScene(0);
            }
            //Debug.Log("인터넷 연결이 끊어졌습니다.");
            StartCoroutine(InternetCheckUI(2.5f, 0.5f, internetCheck));
        }
        else
        {
            if (firstInternetCheck)
            {
                //Debug.Log("인터넷이 연결되었습니다.");
                StartCoroutine(InternetCheckUI(1.0f, 0.5f, internetCheck));
            }
            else
            {
                firstInternetCheck = true;
                return;
            }

        }
    }


    private IEnumerator InternetCheckUI(float showTime, float hideTime, bool internet)
    {
        float time = 0;
        if (internet) internetTMP.text = "네트워크 연결됨";
        else internetTMP.text = "네트워크를 확인해주세요";
        canvasGroup.alpha = 1;
        yield return new WaitForSeconds(showTime);

        while (time < hideTime)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / hideTime);
            canvasGroup.alpha = Mathf.Lerp(1, 0, t);
            yield return null;
        }
        canvasGroup.alpha = 0;
    }

    #region 씬
    /// <summary>
    /// 다른 씬으로 넘어가는데 전 화면으로 돌아가기가 필요하면 사용
    /// </summary>
    public void LoadScene(string sceneName)
    {
        // 현재 씬 이름 저장
        string currentScene = SceneManager.GetActiveScene().name;
        sceneHistory.Push(currentScene);

        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// 현재 씬에서 기존 씬으로 돌아가기
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
            //종료한거냐 묻는 UI 띄우고 OK 누르면 아래 코드로 닫기
            Application.Quit();

        }
    }
    #endregion
}
