using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject menuButton;
    [SerializeField] private GameObject Nl;
    public Animator animator;
    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public void ManuClick()
    {
        menuButton.SetActive(false);
        Nl.SetActive(true);
        //�޴�â ������ ������ �ִϸ��̼� ����
    }

    public void CloseMenu()
    {
        //�ӽ� ���
        menuButton.SetActive(true);
        Nl.SetActive(false);
        //�޴�â�� �ٽ� ���� �ִϸ��̼�
    }
}
