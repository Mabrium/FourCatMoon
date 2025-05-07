using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarpPoint : MonoBehaviour
{
    //�� �ڵ尡 �ִ� ������Ʈ�� ���� ĳ������ ��ġ �̵��̳� �� �̵��� ��Ŵ
    //�� �̵��� �� �ڵ忡�� �����̴ٺ��� �� �̸� �ٲ�� �� ã�ư����ϰų� �ٸ� ����� ã�ƾ���

    [SerializeField] private bool S_P;             //Ȱ��ȭ�� �� �̵� �ƴϸ� ��ġ�̵�
    [SerializeField] private Vector2 P_position;   //P�� position
    [SerializeField] private string S_name;        //S�� Scene



    private void OnTriggerStay2D(Collider2D collision)
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
