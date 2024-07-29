using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SellManager : MonoBehaviour
{
    public ItemSlotUI[] uiSlots; // UI ���� ������ ����
    public ItemSlot[] slots;     // ���� ������ ������ ����Ǵ� �迭

    [Header("Item Information")]
    public TextMeshProUGUI money;
    private ItemSlot selectedItem;
    private int selectedItemIndex;
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedItemStatName;
    public TextMeshProUGUI selectedItemStatValue;
    public TextMeshProUGUI selectedItemPriceName;
    public TextMeshProUGUI selectedItemPriceValue;

    [Header("UI")]
    public GameObject sellPanel;
    public GameObject sellButton;

    FirebaseManager firebaseManager;

    public static SellManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ClearSelectedItemUI();
        // ó���� sellPanel�� Ȱ��ȭ ���¸� �ʱ�ȭ
        sellPanel.SetActive(false);

        firebaseManager = GameManager.Instance.FirebaseManager;
        firebaseManager.LoadUserItems(OnItemsLoaded);
    }

    private bool wasSellPanelActive = false;

    private void Update()
    {
        // sellPanel�� Ȱ��ȭ ���°� ����Ǿ����� Ȯ��
        if (sellPanel.activeSelf && !wasSellPanelActive)
        {
            // sellPanel�� Ȱ��ȭ�Ǿ��� �� LoadUserItems ȣ��
            wasSellPanelActive = true;
            firebaseManager.LoadUserItems(OnItemsLoaded);
        }
        else if (!sellPanel.activeSelf && wasSellPanelActive)
        {
            wasSellPanelActive = false;
        }
    }

    private void OnItemsLoaded(List<ItemData> items)
    {
        Debug.Log("on items loaded");

        // ���� �ʱ�ȭ
        slots = new ItemSlot[uiSlots.Length];
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = new ItemSlot();
            uiSlots[i].index = i;
            uiSlots[i].Clear();
        }

        ClearSelectedItemUI();

        // Fill the sell market slots with loaded items
        for (int i = 0; i < items.Count && i < slots.Length; i++)
        {
            slots[i].item = items[i];
            slots[i].quantity = items[i].quantity;
            uiSlots[i].Set(slots[i]);
        }
    }

    public void SelectItem(int index)
    {
        if (slots[index].item == null) return;

        selectedItem = slots[index];
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.item.displayName;
        selectedItemDescription.text = selectedItem.item.description;
        selectedItemStatName.text = "HP";
        selectedItemStatValue.text = selectedItem.item.consumableValue.ToString();
        selectedItemPriceName.text = "Price";
        selectedItemPriceValue.text = selectedItem.item.itemPrice.ToString();

        if (selectedItem.item.attackDefense != null)
        {
            for (int i = 0; i < selectedItem.item.attackDefense.Length; i++)
            {
                selectedItemStatName.text += "\n" + selectedItem.item.attackDefense[i].type.ToString();
                selectedItemStatValue.text += "\n" + selectedItem.item.attackDefense[i].value.ToString();
            }
        }

        sellButton.SetActive(true);
    }

    void UpdateUI()
    {
        List<ItemSlot> filledSlots = new List<ItemSlot>();
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                filledSlots.Add(slots[i]);
            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < filledSlots.Count)
            {
                slots[i] = filledSlots[i];
                uiSlots[i].Set(slots[i]);
            }
            else
            {
                slots[i] = new ItemSlot();
                uiSlots[i].Clear();
            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
                uiSlots[i].Set(slots[i]);
            else
                uiSlots[i].Clear();
        }
    }

    private void ClearSelectedItemUI()
    {
        UpdateMoneyUI();
        selectedItem = null;
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;
        selectedItemPriceName.text = string.Empty;
        selectedItemPriceValue.text = string.Empty;

        sellButton.SetActive(false);
    }

    private void RemoveSelectedItem()
    {
        selectedItem.quantity--;
        firebaseManager.DeleteItemData(selectedItem.item); // ������ ����

        if (selectedItem.quantity <= 0)
        {
            slots[selectedItemIndex].item = null;
            ClearSelectedItemUI();
        }
        else
        {
            uiSlots[selectedItemIndex].Set(slots[selectedItemIndex]);
        }

        UpdateUI();
    }

    private void UpdateMoneyUI()
    {
        money.text = MoneyManager.Money.ToString();
    }

    public void OnSellButton()
    {
        if (selectedItem == null) return;

        MoneyManager.Earn(selectedItem.item.itemPrice);
        RemoveSelectedItem();
        Inventory.instance.ClearSelectItemWindow();
        UpdateMoneyUI();
    }

    public void OnPanelExitBtn()
    {
        sellPanel.SetActive(false);
    }
}
