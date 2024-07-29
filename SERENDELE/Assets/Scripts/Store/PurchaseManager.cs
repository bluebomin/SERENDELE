using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PurchaseManager : MonoBehaviour
{
    public static PurchaseManager instance;

    public ItemSlotUI[] uiSlots; // UI ���� ������ ����
    public ItemSlot[] slots;     // ���� ������ ������ ����Ǵ� �迭
    public ItemData[] shopItems; // ������ ����� ������ �迭

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
    public GameObject purchasePanel;
    public GameObject purchaseButton;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        slots = new ItemSlot[uiSlots.Length];
        for (int i = 0; i < slots.Length; i++)
        {
            // UI Slot �ʱ�ȭ�ϱ�
            slots[i] = new ItemSlot();
            uiSlots[i].index = i;
            uiSlots[i].Clear();
        }

        // ���� ������ �ʱ�ȭ
        InitializeShopItems();

        UpdateMoneyUI();
        ClearSelectedItemUI();
    }

    private void InitializeShopItems()
    {
        for (int i = 0; i < shopItems.Length && i < slots.Length; i++)
        {
            slots[i].item = shopItems[i];
            slots[i].quantity = 1;
            uiSlots[i].Set(slots[i]);
        }
    }

    public void SelectItem(int index)
    {
        // ������ ���Կ� �������� ���� ���
        if (slots[index].item == null) return;

        // ������ ���� �ҷ�����
        selectedItem = slots[index];
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.item.displayName;
        selectedItemDescription.text = selectedItem.item.description;
        selectedItemPriceName.text = "Price";
        selectedItemPriceValue.text = selectedItem.item.itemPrice.ToString();

        selectedItemStatName.text = "HP";
        selectedItemStatValue.text = selectedItem.item.consumableValue.ToString();

        for (int i = 0; i < selectedItem.item.attackDefense.Length; i++)
        {
            selectedItemStatName.text += selectedItem.item.attackDefense[i].type.ToString() + "\n";
            selectedItemStatValue.text += selectedItem.item.attackDefense[i].value.ToString() + "\n";
        }

        purchaseButton.SetActive(true);
    }

    private void ClearSelectedItemUI()
    {
        selectedItem = null;
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;
        selectedItemPriceName.text = string.Empty;
        selectedItemPriceValue.text = string.Empty;

        purchaseButton.SetActive(false);
    }

    private void UpdateMoneyUI()
    {
        money.text = MoneyManager.Money.ToString();
    }

    public void OnPurchaseButton()
    {
        if (selectedItem == null) return;

        if (MoneyManager.Money < selectedItem.item.itemPrice)
        {
            Debug.LogError("No Money");
            return;
        }
        MoneyManager.Spend(selectedItem.item.itemPrice);
        Inventory.instance.AddItem(selectedItem.item);
        uiSlots[selectedItemIndex].Disable();
        UpdateMoneyUI();
    }

    public void OnPanelExitBtn()
    {
        purchasePanel.SetActive(false);
    }
}
