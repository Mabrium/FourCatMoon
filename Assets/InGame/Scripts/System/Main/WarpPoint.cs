using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarpPoint : MonoBehaviour
{
    //이 코드가 있는 오브젝트를 밟은 캐릭터가 상호작용시 위치 이동이나 씬 이동을 시킴
    //씬 이동은 각 코드에서 적용이다보니 씬 이름 바뀌면 잘 찾아가야하거나 다른 방법을 찾아야함

    [SerializeField] private bool S_P;             //활성화시 씬 이동 아니면 위치이동
    [SerializeField] private Vector2 P_position;   //P는 position
    [SerializeField] private string S_name;        //S는 Scene


    private void OnTriggerEnter2D(Collider2D collision)
    {
        

        if (S_P)
        {
            SceneManager.LoadScene(S_name);
        }
        else
        {
            GameObject PlayerObject = collision.gameObject;
            PlayerObject.transform.position = P_position;
        }
    }
}
