using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Data;

public class CharacterSpawner : MonoBehaviour
{
    [SerializeField] private CharacterData characterData;

    void Awake()
    {

    }

    void Start()
    {
        StartRandomStatistics();
    }

    void Update()
    {
        
    }

    //���߿� ĳ������ �Ӽ�(��)�� ���� �ٸ��� ���� ��ġ�� �ٲٱ�
    public void StartRandomStatistics()
    {
        characterData.atk = Random.Range(3, 6);
        characterData.def = Random.Range(4, 6);
        characterData.maxHp = Random.Range(23, 25);
        characterData.speed = Random.Range(10, 12);
    }

    public void SaveStatistics()
    {

    }

}
