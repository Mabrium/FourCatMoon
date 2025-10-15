using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Encounter : MonoBehaviour
{
    private int randFight;

    private float progress;
    private int aProgress;

    void Start()
    {

    }

    void Update()
    {

    }

    //Ç®½£¿¡ ´ê¾ÒÀ» ¶§ ·£´ý ÀçÈ¸
    private void OnTriggerEnter2D(Collider2D collision)
    {
        randFight = Random.Range(0, 100);
        if (randFight >= 90)
        {
            StartCoroutine(Fight());
        }
    }

    private IEnumerator Fight()
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
