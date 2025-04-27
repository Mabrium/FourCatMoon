using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaManager : MonoBehaviour
{
    private GameObject cat;
    public GameObject[] prefabObject;
    public GameObject alpha;
    [SerializeField] private RectTransform rectTransform;

    public List<CharacterSpawner> spawners = new List<CharacterSpawner>();

    public int i = 0;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

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
        int num = 0;
        int ma = 300;
        if (GachaSelect.gacha10pOption)
        {
            for (i = 1; i < 11; i++)
            {
                Spawn();
                cat.transform.localPosition = new Vector3(-700f + (num * 350f), ma, 0);
                num++;
                if(i == 5)
                {
                    num = 0;
                    ma = -165;
                }
            }
            Debug.Log("10��");
        }
        //1���� ��
        else if (!GachaSelect.gacha10pOption)
        {
            Spawn();
            Debug.Log("1��");
        }
    }

    private void Spawn()
    {
        cat = Instantiate(prefabObject[i-1], rectTransform.anchoredPosition, Quaternion.identity, rectTransform.transform);
        CharacterSpawner PrefabScript = cat.GetComponent<CharacterSpawner>();
        spawners.Add(PrefabScript);
    }

    public void TouchCard()
    {
        alpha.SetActive(!alpha.activeSelf);
    }
}
