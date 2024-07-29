using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class HpAndExp : MonoBehaviour
{
    // ����
    public float maxHp;  // �ִ� HP
    public float curHp;   // ���� HP
    public float curExp;  // ���� ����ġ
    public int curLevel;  // ���� ����
    public int offense;   // ���ݷ�
    public int defense;   // ����

    // UI
    private TextMeshProUGUI levelTxt;
    private TextMeshProUGUI HpTxt;
    private TextMeshProUGUI ExpTxt;
    private Slider HPBar;
    private Slider ExpBar;
    private Image HPBarFillImage;

    private Dictionary<int, int> experienceTable;
    private DatabaseReference databaseReference;

    private FirestoreManager firestoreManager;

    private void Start()
    {
        firestoreManager = FindObjectOfType<FirestoreManager>();
        StartCoroutine(SaveDataPeriodically());

        // ����ġ ���̺�
        experienceTable = new Dictionary<int, int>();
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
                LoadExperienceTableFromFirebase();
                firestoreManager.LoadData(UpdatePlayerData);
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + task.Result);
            }
        });

        // UI ã��
        levelTxt = GameObject.Find("level_status").GetComponent<TextMeshProUGUI>();
        if (levelTxt != null) Debug.Log("levelTxt found and assigned.");
        else Debug.LogError("levelTxt not found!");
        HpTxt = GameObject.Find("hp_status").GetComponent<TextMeshProUGUI>();
        if (HpTxt != null) Debug.Log("HpTxt found and assigned.");
        else Debug.LogError("HpTxt not found!");
        ExpTxt = GameObject.Find("exp_status").GetComponent<TextMeshProUGUI>();
        if (ExpTxt != null) Debug.Log("ExpTxt found and assigned.");
        else Debug.LogError("ExpTxt not found!");
        HPBar = GameObject.Find("HP_bar").GetComponent<Slider>();
        if (HPBar != null) Debug.Log("HPBar found and assigned.");
        else Debug.LogError("HPBar not found!");
        ExpBar = GameObject.Find("EXP_bar").GetComponent<Slider>();
        if (ExpBar != null) Debug.Log("ExpBar found and assigned.");
        else Debug.LogError("ExpBar not found!");

        // HPBar �⺻�� �ʱ�ȭ
        HPBar.minValue = 0;
        HPBar.maxValue = 1;

        // HPBar ����
        HPBarFillImage = HPBar.fillRect.GetComponent<Image>();
    }

    private void Update()
    {
        // HP�� ����ȭ
        HPBar.value = curHp / maxHp;
        HpTxt.text = $"{(int)curHp}";

        // HPBar ���� ����
        if (HPBar.value <= 0.2f)
        {
            HPBarFillImage.color = Color.red;
        }
        else
        {
            HPBarFillImage.color = new Color(0.13f, 0.69f, 0.2f); // 22B033 ����
        }

        // EXP�� ����ȭ
        int maxExpForCurrentLevel = GetExperienceForLevel(curLevel);
        ExpBar.value = maxExpForCurrentLevel > 0 ? curExp / maxExpForCurrentLevel : 0;
        ExpTxt.text = $"{(int)curExp}";

        // ���� �ؽ�Ʈ ����ȭ
        levelTxt.text = $"{curLevel}";
    }

    // Firebase�� �ִ� ����ġ ���̺� ��������
    private void LoadExperienceTableFromFirebase()
    {
        databaseReference.Child("experience_table").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error getting data from Firebase: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (var child in snapshot.Children)
                {
                    int level = int.Parse(child.Key);
                    int experience = int.Parse(child.Value.ToString());
                    experienceTable[level] = experience;
                }
                Debug.Log("Experience table loaded successfully.");
            }
        });
    }

    // experienceTable�� �ִ� ���� �� �ʿ� ����ġ ��������
    public int GetExperienceForLevel(int level)
    {
        if (experienceTable.ContainsKey(level))
        {
            return experienceTable[level];
        }
        return 0;
    }

    // �ֱ������� ������ ����
    private IEnumerator SaveDataPeriodically()
    {
        while (true)
        {
            yield return new WaitForSeconds(600); // 10�� ���
            firestoreManager.SaveData(maxHp, curHp, curExp, curLevel, offense, defense);
        }
    }

    // �÷��̾� ������ ������Ʈ�ϱ�
    private void UpdatePlayerData(float maxhp, float curhp, float exp, int level, int off, int def)
    {
        maxHp = maxhp;
        curHp = curhp;
        curExp = exp;
        curLevel = level;
        offense = off;
        defense = def;
        Debug.Log($"Player data loaded: MaxHP={maxhp}, CurHp={curhp}, EXP={exp}, Level={level}, Offense={offense}, Defense={defense}");
    }

    // HP ȸ��
    public void IncreaseHp(float amount)
    {
        curHp += amount;
        if (curHp > maxHp)
        {
            curHp = maxHp;
        }
        Debug.Log($"HP increased by {amount}. Current HP: {curHp}");
    }

    // HP ����(���� ��)
    public void DecreaseHp(float amount)
    {
        curHp -= amount * (float)(1 - 0.01 * defense);
        if (curHp < 0)
        {
            curHp = 0;
            Debug.Log("Game Over");
        }
    }

    // ����ġ ����
    public void IncreaseExp(float amount)
    {
        curExp += amount;
        CheckForLevelUp();
    }

    // ������ �ؾ��ϴ��� Ȯ��
    private void CheckForLevelUp()
    {
        int requiredExperience = GetExperienceForLevel(curLevel);
        if (curExp >= requiredExperience)
        {
            curExp -= requiredExperience;
            LevelUp();
            firestoreManager.SaveData(maxHp, curHp, curExp, curLevel, offense, defense);  // ������ �ϸ� ������ ����
        }
    }

    // ������
    private void LevelUp()
    {
        curLevel++;
        IncreaseStatsForLevelUp();
        curHp = maxHp;
    }

    // ������ �� ���� ���
    public void IncreaseStatsForLevelUp()
    {
        maxHp += 50;
        offense += 4;
        defense += 4;
    }
}
