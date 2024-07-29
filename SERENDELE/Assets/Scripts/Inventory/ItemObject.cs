using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData item;

    public string GetInteractPrompt()
    {
        return string.Format("Pick up {0}", item.displayName);
    }

    public void OnInteract()
    {
        Inventory.instance.AddItem(item);   // �κ��丮�� ������ �߰��ϱ�
        Destroy(gameObject);
    }
}