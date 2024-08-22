using Firebase.Auth;
using TMPro;
using UnityEngine;
using Cinemachine;

public interface IInteractable
{
    string GetInteractPrompt();
    void OnInteract();
}

public class InteractionManager : MonoBehaviour
{
    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxCheckDistance;
    public LayerMask layerMask;

    private GameObject curInteractGameobject;
    private IInteractable curInteractable;

    public GameObject promptBg;
    public TextMeshProUGUI promptText;
    public CinemachineVirtualCamera virtualCamera;

    private Camera mainCamera;

    void Update()
    {
        if (!Inventory.instance.inventoryWindow.activeInHierarchy)
        {
            // E Ű �Է� �� �κ��丮��
            if (Input.GetKeyDown(KeyCode.E))
            {
                Interact();
            }
        }

        // ���������� üũ�� �ð��� checkRate�� �Ѱ�ٸ�
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;
            CheckForInteractable();
        }
    }

    private void Start()
    {
        mainCamera = Camera.main;

        if (virtualCamera == null)
        {
            Debug.LogError("CinemachineVirtualCamera�� �Ҵ���� �ʾҽ��ϴ�.");
            return;
        }
    }

    private void CheckForInteractable()
    {
        // ȭ���� �� �߾ӿ� ��ȣ�ۿ� ������ ��ü�� �ִ��� Ȯ���ϱ�
        Ray centerRay = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));    // ȭ���� �� �߾ӿ��� Ray�� ��ڴ�.
        Ray upRay = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2 + Mathf.Tan(15 * Mathf.Deg2Rad) * maxCheckDistance));
        Ray downRay = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2 - Mathf.Tan(15 * Mathf.Deg2Rad) * maxCheckDistance));

        bool hitDetected = PerformRaycast(centerRay) || PerformRaycast(upRay) || PerformRaycast(downRay);

        if (!hitDetected)
        {
            curInteractGameobject = null;
            curInteractable = null;
            promptBg.gameObject.SetActive(false);
        }
    }

    private bool PerformRaycast(Ray ray)
    {
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
        {
            // ����� ������ ����Ͽ� Ray�� �ð�ȭ�մϴ�.
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);

            // �ε��� ������Ʈ�� �츮�� �����س��� ��ȣ�ۿ��� ������ ������Ʈ������ Ȯ���ϱ�
            if (hit.collider.gameObject != curInteractGameobject)
            {
                // �浹�� ��ü ��������
                curInteractGameobject = hit.collider.gameObject;
                curInteractable = hit.collider.GetComponent<IInteractable>();
                SetPromptText();
            }
            return true;
        }
        else
        {
            // ����� ������ ����Ͽ� Ray�� �ð�ȭ�մϴ�.
            Debug.DrawRay(ray.origin, ray.direction * maxCheckDistance, Color.red);
            return false;
        }
    }

    private void SetPromptText()
    {
        promptBg.gameObject.SetActive(true);
        promptText.text = string.Format("<b>[E]</b> {0}", curInteractable.GetInteractPrompt());     // <b></b> : �±�, ��ũ�ٿ� ���� <b>�� ��� ����ü.
    }

    private void Interact()
    {
        if (curInteractable != null)
        {
            curInteractable.OnInteract();
        }
    }
}