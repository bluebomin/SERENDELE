using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceManager : MonoBehaviour
{
    public static ChoiceManager Instance;

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

    public GameObject choicePanel;

    private List<string> answerList;

    public TMP_Text questionTxt;
    public TMP_Text[] answerTxt;
    public Button[] answerBtn;
    public RawImage[] answerTri;

    public bool choiceIng;  // ���
    private bool keyInput;  // Űó�� Ȱ��ȭ ����

    private int count;      // �迭�� ũ��
    public int result;     // ������ �亯

    private void Start()
    {
        answerList = new List<string>();

        for (int i = 0; i < answerTxt.Length; i++)
        {
            answerTxt[i].text = "";
            answerTri[i].gameObject.SetActive(false);
        }
        questionTxt.text = "";

        choicePanel.SetActive(false);
    }

    public void ShowChoice(Choice _choice)
    {
        result = 0;  // �ʱ⿡�� ������ ���� ���·� ����
        questionTxt.text = _choice.question;
        for (int i = 0; i < _choice.answers.Length; i++)
        {
            answerList.Add(_choice.answers[i]);
            answerTxt[i].text = _choice.answers[i];
            count = i;
        }
        choicePanel.SetActive(true);

        Selection();
        keyInput = true; // Ű �Է� ���
    }

    private void Update()
    {
        if (keyInput)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (result > 0)
                {
                    result--;
                }
                else
                {
                    result = count;
                }
                Selection();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (result < count)
                {
                    result++;
                }
                else
                {
                    result = 0;
                }
                Selection();
            }
            else if (Input.GetKeyDown(KeyCode.H))
            {
                keyInput = false;
                choiceIng = false;
            }
        }
    }

    public void Selection()
    {
        Color color = new Color(1f, 1f, 1f);
        for (int i = 0; i <= count; i++)
        {
            answerTxt[i].GetComponent<TextMeshProUGUI>().color = color;
            answerTxt[i].GetComponent<TextMeshProUGUI>().margin = new Vector4(25f, 8f, 0f, 8f);
            answerTri[i].gameObject.SetActive(false);
        }

        Color newColor = new Color(1f, 1f, 0.725f);
        answerTxt[result].GetComponent<TextMeshProUGUI>().color = newColor;
        answerTxt[result].GetComponent<TextMeshProUGUI>().margin = new Vector4(80f, 8f, 0f, 8f);
        answerTri[result].gameObject.SetActive(true);
    }

    public void ExitChoice()
    {
        for (int i = 0; i <= count; i++)
        {
            answerTxt[i].text = "";
        }
        choicePanel.SetActive(false);
        choiceIng = false;
        questionTxt.text = "";
        answerList.Clear();
    }

    public int GetResult()
    {
        return result + 1;
    }
}
