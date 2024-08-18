using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    Dictionary<int, string[]> talkData;  // ĳ���� id, ���
    int randomChoiceKey;

    private void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        GenerateData();
    }

    void GenerateData()
    {
        /* 100��: NPC �̸�, 10��: ��� ������ ����, 1��: ��� */
        // 100: Rio
        talkData.Add(100, new string[] {
            "�ȳ�? ����� ó�� �Ա���?",
            "������ �� �� ������ �� �� ȯ����." });
        talkData.Add(100 + 10 + 1, new string[] {
            "���� �� ������ ������ ������ �ϰ� �;�.",
            "�׷��� ���� ������ ������..." });
        talkData.Add(100 + 10 + 2, new string[] {
            "���� ���� ���� ���� �� ä���ؿ��� ��.",
            "���� ������ �ʾ�."});
        talkData.Add(100 + 10 + 3, new string[] {
            "���� ���� ����� ������ ���⿡ ��Ҿ�.",
            "���⿡�� �¾���� �ʾ����� ���� �� �����̳� ����������."});

    }

    public string GetTalk(int id, int index, int talkIndex)
    {
        int baseKey = id + index;
        string selectedString = null;

        // ������ �� �ϳ��� �������� ����
        if (talkIndex == 0)
        {
            // ó�� ����� ��� randomChoiceKey�� ���� �����ϰ� ����
            randomChoiceKey = baseKey + Random.Range(1, 4);
        }

        // �⺻ ��� ��������
        if (talkData.ContainsKey(baseKey))
        {
            if (talkIndex == talkData[baseKey].Length)
            {
                selectedString = null;
            }
            else
            {
                selectedString = talkData[baseKey][talkIndex];
            }
        }

        // ����� randomChoiceKey�� ����Ͽ� ��� ��������
        if (talkData.ContainsKey(randomChoiceKey))
        {
            if (talkIndex < talkData[randomChoiceKey].Length)
            {
                selectedString = talkData[randomChoiceKey][talkIndex];
            }
        }

        return selectedString;
    }
}
