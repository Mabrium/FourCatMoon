using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMove : MonoBehaviour
{
    public void SceneMoveInstall(string SceneName)
    {
        if (SceneName.Equals("CharacterScene"))
        {
            Manager.Instance.LoadSceneAsync(SceneName);
        }
        else
        {
            Manager.Instance.LoadScene(SceneName);
        }
    }

    public void BackScene()
    {
        Manager.Instance.GoBack();
    }
}
