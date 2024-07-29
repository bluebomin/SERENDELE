using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using TMPro;
using System.Threading.Tasks;

public class AuthManager : MonoBehaviour
{
    [Header("Log in")]
    [SerializeField] GameObject LoginPanel;
    [SerializeField] TMP_InputField emailField;
    [SerializeField] TMP_InputField passField;
    [SerializeField] Button loginBtn;
    [SerializeField] GameObject FailLog;
    [SerializeField] Button RegisterTxt;

    [Header("Register")]
    [SerializeField] GameObject RegisterPanel;
    [SerializeField] TMP_InputField RegisterEmailField;
    [SerializeField] Button RegisterEmailCheckBtn;
    [SerializeField] TMP_Text RegisterEmailCheckTxt;
    [SerializeField] TMP_InputField RegisterPassField;
    [SerializeField] TMP_InputField RegisterPassCheckField;
    [SerializeField] TMP_Text RegisterPassCheckTxt;
    [SerializeField] TMP_InputField NicknameField;
    [SerializeField] Button RegisterBtn;
    [SerializeField] GameObject RegisterToLogin;

    // ������ ������ ��ü
    private FirebaseAuth auth;

    private bool showFailLogCoroutine;

    private FirestoreManager firestoreManager;

    void Awake()
    {
        // ��й�ȣ �� ���̰� ó��
        passField.contentType = TMP_InputField.ContentType.Password;
        RegisterPassField.contentType = TMP_InputField.ContentType.Password;
        RegisterPassCheckField.contentType = TMP_InputField.ContentType.Password;
    }

    void Start()
    {
        firestoreManager = FindObjectOfType<FirestoreManager>();

        // Firebase �ʱ�ȭ
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
            }
            else
            {
                Debug.LogError("Firebase: Could not resolve all Firebase dependencies: " + task.Result);
            }
        });

        // ��й�ȣ �Է��ϴ� �� üũ
        RegisterPassField.onValueChanged.AddListener(delegate { CheckPasswords(); });
        RegisterPassCheckField.onValueChanged.AddListener(delegate { CheckPasswords(); });

        RegisterEmailCheckTxt.text = string.Empty;
        RegisterPassCheckTxt.text = string.Empty;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            HandleTabPress();
        }

        if (showFailLogCoroutine)
        {
            showFailLogCoroutine = false;
            StartCoroutine(ShowFailLog());
        }
    }

    // ��Ű ������ ĭ ������
    void HandleTabPress()
    {
        if (emailField.isFocused)
        {
            EventSystem.current.SetSelectedGameObject(passField.gameObject);
        }
        else if (passField.isFocused)
        {
            EventSystem.current.SetSelectedGameObject(loginBtn.gameObject);
        }
    }

    public void Login()
    {
        // �̸��ϰ� ��й�ȣ�� �α��� ���� ��
        auth.SignInWithEmailAndPasswordAsync(emailField.text, passField.text).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
            {
                FirebaseUser user = task.Result.User; // Get the FirebaseUser object from the task result
                string nickname = user.DisplayName;
                Debug.Log($"{user.Email} �� �α��� �ϼ̽��ϴ�. �г���: {nickname}");

                // Update UI
                NicknameField.text = nickname;

                // �⺻������ �Է�
                firestoreManager.CheckAndSaveDefaultData(user.UserId);
            }
            else
            {
                Debug.Log("�α��ο� �����ϼ̽��ϴ�.");
                showFailLogCoroutine = true;
            }
        });
    }

    private IEnumerator ShowFailLog()
    {
        FailLog.SetActive(true);
        // ��ȣ�ۿ� ��Ȱ��ȭ
        emailField.interactable = false;
        passField.interactable = false;
        loginBtn.interactable = false;
        RegisterTxt.interactable = false;

        // Wait for 2 seconds
        yield return new WaitForSeconds(2);

        FailLog.SetActive(false);
        // ��ȣ�ۿ� Ȱ��ȭ
        emailField.interactable = true;
        passField.interactable = true;
        loginBtn.interactable = true;
        RegisterTxt.interactable = true;
    }

    public void GoToRegister()
    {
        LoginPanel.SetActive(false);
        emailField.text = string.Empty;
        passField.text = string.Empty;
        RegisterPanel.SetActive(true);
    }

    public void Register()
    {
        string email = RegisterEmailField.text;
        string password = RegisterPassField.text;
        string nickname = NicknameField.text;

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.Log("ȸ������ ����: " + task.Exception);
                return;
            }

            FirebaseUser newUser = task.Result.User;  // AuthResult���� FirebaseUser ��������
            UpdateUserProfile(newUser, nickname);
        });
    }

    private void UpdateUserProfile(FirebaseUser user, string nickname)
    {
        UserProfile profile = new UserProfile { DisplayName = nickname };

        user.UpdateUserProfileAsync(profile).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.Log("������ ������Ʈ ����: " + task.Exception);
                return;
            }

            Debug.Log("ȸ������ �� ������ ������Ʈ ����!");
            StartCoroutine(ShowRegisterToLogin());
        });
    }

    private IEnumerator ShowRegisterToLogin()
    {
        RegisterPanel.SetActive(false);
        RegisterToLogin.SetActive(true);

        yield return new WaitForSeconds(2);

        RegisterToLogin.SetActive(false);
        RegisterPanel.SetActive(false);
        LoginPanel.SetActive(true);

        RegisterEmailField.text = string.Empty;
        RegisterPassField.text = string.Empty;
        RegisterPassCheckField.text = string.Empty;
        NicknameField.text = string.Empty;
    }

    private void CheckPasswords()
    {
        if (RegisterPassField.text == RegisterPassCheckField.text)
        {
            RegisterPassCheckTxt.text = "��й�ȣ�� ��ġ�մϴ�";
            RegisterPassCheckTxt.color = Color.green;
        }
        else
        {
            RegisterPassCheckTxt.text = "��й�ȣ�� ��ġ���� �ʽ��ϴ�";
            RegisterPassCheckTxt.color = Color.red;
        }
    }
}
