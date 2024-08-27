using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class NPCObjct : MonoBehaviour, IInteractable
{
    public NPCData npcData;
    public string GetInteractPrompt()
    {
        Debug.Log(npcData.NPCName);
        return string.Format("{0}��(��) ��ȭ�ϱ�", npcData.NPCName);
    }

    public void OnInteract()
    {
        Debug.Log("NPC ��ȭ");
    }
}
