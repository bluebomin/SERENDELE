using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomListItem : MonoBehaviour
{
    public TMP_Text roomInfo;

    // Ŭ���Ǿ��� �� ȣ��Ǵ� �Լ�
    public Action<string> onDelegate;

    // InputField ������ �����ϴ� ����
    private TMP_InputField inputServerNameField;

    void Start()
    {
        // InputField�� �������� ã���ϴ�
        GameObject go = GameObject.Find("InputServerName");
        if (go != null)
        {
            inputServerNameField = go.GetComponent<TMP_InputField>();
            Debug.Log("ã��" + inputServerNameField.name);
        }
        else
        {
            Debug.LogError("InputServerName GameObject�� ã�� �� �����ϴ�.");
        }
    }

    public void SetInfo(string roomName, int currPlayer, int maxPlayer)
    {
        name = roomName;
        roomInfo.text = "  " + roomName + "  (" + currPlayer + '/' + maxPlayer + ")";
    }

    public void OnClick()
    {
        // ���� onDelegate �� ���� ����ִٸ� ����
        if (onDelegate != null)
        {
            onDelegate(name);
        }
        Debug.Log("InputField�� �ؽ�Ʈ ����: " + name);
        inputServerNameField.text = name;
    }
}
