using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    Dictionary<int, string[]> talkData;  // ĳ���� id, ���

    private void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        GenerateData();
    }

    void GenerateData()
    {
        /* 100��: NPC �̸�, 10��: ��� ������ ����, 1��: ��� */
        // 100: Rio
        talkData.Add(100 + 10, new string[] { 
            "�ȳ�? ����� ó�� �Ա���?", 
            "�������� �� �� ȯ����." });
        talkData.Add(100 + 20 + 1, new string[] { 
            "���� �� ������ ������ ������ �ϰ� �;�.", 
            "�׷��� ���� ������ ������..." });
        talkData.Add(100 + 20 + 2, new string[] {
            "���� ���� ���� ���� �� ä���ؿ��� ��.",
            "���� ������ �ʾ�."});
        talkData.Add(100 + 20 + 3, new string[] {
            "���� ���� ����� ������ ���⿡ ��Ҿ�.",
            "���⿡�� �¾���� �ʾ����� ���� �� �����̳� ����������."});

    }

    public string GetTalk(int id, int talkIndex)
    {
        if (!talkData.ContainsKey(id))  // �����Ͱ� �����ϴ��� �˻�
        {

            if (!talkData.ContainsKey(id - id % 10))
            {
                // ����Ʈ �� ó�� ��縶�� ���� ���, �⺻ ��縦 ������ ��
                if (talkIndex == talkData[id - id % 100].Length)
                {
                    return null;
                }
                else
                {
                    return talkData[id - id % 100][talkIndex];
                }
            }
            else
            {
                // �ش� ����Ʈ ���� ���� ��簡 ���� ��, ����Ʈ �� ó�� ��縦 ������ ��
                if (talkIndex == talkData[id - id % 10].Length)  // ID�� ������ ����Ʈ ��ȭ���� ���� �� ��Ž��
                {
                    return null;
                }
                else
                {
                    return talkData[id - id % 10][talkIndex];
                }
            }
        }

        if (talkIndex == talkData[id].Length)
        {
            return null;
        }
        else
        {
            return talkData[id][talkIndex];
        }
    }
}
