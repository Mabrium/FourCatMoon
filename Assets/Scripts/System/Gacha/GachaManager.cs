using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaManager : MonoBehaviour
{
    public GameObject[] characterSpawners;
    public List<CharacterSpawner> spawners = new List<CharacterSpawner>();
    void Start()
    {
        PrefabMove();
    }

    void Update()
    {

    }

    private void PrefabMove()
    {

        //ũ�� �ٲٱ� �̰� ����Ǹ� ������ 10���� �����ϰ� ������ �� ���� �� �����͸� �����ϱ�

        //10���� ��
        if (GachaSelect.gacha10pOption)
        {
            for (int i = 0; i < characterSpawners.Length; i++)
            {
                spawners.Add(characterSpawners[i].GetComponent<CharacterSpawner>());
            }
            Debug.Log("10��");
        }
        //1���� ��
        else if (!GachaSelect.gacha10pOption)
        {
            spawners.Add(characterSpawners[0].GetComponent<CharacterSpawner>());
            Debug.Log("1��");
        }
    }
}
