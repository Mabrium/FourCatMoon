using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterManager : MonoBehaviour
{

    private float nowPositionX;
    private float nowPositionY;

    [SerializeField] private Encounter[] encounters;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public IEnumerator Fight()
    {
        //Debug.Log("Fight");
        StartCoroutine(Manager.Instance.Fade());
        yield return new WaitForSeconds(2.5f);
        Manager.Instance.LoadScene("BattleScene");
        //AsyncOperation asyncOperationScene = SceneManager.LoadSceneAsync("BattleScene");
        //asyncOperationScene.allowSceneActivation = false;
        //while (!asyncOperationScene.isDone)
        //{
        //    progress = Mathf.Clamp01(asyncOperationScene.progress / 0.9f);
        //    aProgress = Mathf.RoundToInt(progress * 100);
        //    if (progress >= 1f)
        //    {
        //        asyncOperationScene.allowSceneActivation = true;
        //    }
        //    yield return null;
        //}
        //yield return new WaitForSeconds(0.1f);

    }
}
