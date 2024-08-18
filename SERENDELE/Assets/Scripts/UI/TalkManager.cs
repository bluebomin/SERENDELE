using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TalkManager : MonoBehaviour
{
    public static TalkManager Instance;
    #region Singleton
    private void Awake()
    {
        if (Instance == null)
        {
            //DontDestroyOnLoad(this.gameObject);
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion Singleton

    public Line lineManager;
    public GameObject talkPanel;       // ���� ��ȭâ
    public TextMeshProUGUI talkText;   // ����â�� �ߴ� �ؽ�Ʈ
    public TextMeshProUGUI nameText;   // NPC �̸�
    public GameObject scanNPC;         // ������ NPC
    public bool isAction;              // ��������� ����
    public int talkIndex;

    private string currentTalks;     // ���� ��� ���� ��� ����Ʈ

    public void Action(GameObject scanObj)
    {
        scanNPC = scanObj;
        ObjectData objData = scanObj.GetComponent<ObjectData>();

        // NPC ��� �ʱ�ȭ
        currentTalks = lineManager.GetTalk(objData.Id, objData.index, talkIndex);

        // ��ȭ ����
        Talk();
    }

    void Talk()
    {
        if (currentTalks == null)
        {
            isAction = false;
            talkIndex = 0;
            talkPanel.SetActive(false);  // ��ȭâ �ݱ�
            return;
        }

        talkPanel.SetActive(true);
        talkText.text = currentTalks;
        nameText.text = scanNPC.GetComponent<ObjectData>().Name;

        isAction = true;

        talkIndex++;
    }
}
