using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TalkManager : MonoBehaviour
{
    public Line lineManager;
    public GameObject talkPanel;       // ���� ��ȭâ
    public TextMeshProUGUI talkText;   // ����â�� �ߴ� �ؽ�Ʈ
    public TextMeshProUGUI nameText;   // NPC �̸�
    public GameObject scanNPC;         // ������ NPC
    public bool isAction;              // ��������� ����
    public int talkIndex;

    public void Action(GameObject scanObj)
    {
        scanNPC = scanObj;
        ObjectData objData = scanObj.GetComponent<ObjectData>();
        Talk(objData.Id);
    }

    void Talk(int id)
    {
        string talkData = lineManager.GetTalk(id, talkIndex);

        if (talkData == null)
        {
            isAction = false;
            talkIndex = 0;
            return;
        }

        talkText.text = talkData;
        nameText.text = ObjectData.Name;
        isAction = true;
        talkIndex++;
    }

    void Update()
    {
        // ���콺 Ŭ�� ����
        if (Input.GetMouseButtonDown(0))
        {
            // Ŭ���� ��ġ�� ������Ʈ Ȯ��
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

            if (hit.collider != null)
            {
                GameObject clickedObject = hit.collider.gameObject;
                // Ŭ���� ������Ʈ�� ObjectData ��ũ��Ʈ�� �ִ��� Ȯ��
                ObjectData objData = clickedObject.GetComponent<ObjectData>();
                if (objData != null)
                {
                    Action(clickedObject);
                }
            }
        }
    }
}
