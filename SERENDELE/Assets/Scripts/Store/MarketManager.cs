using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MarketManager : MonoBehaviour
{
    public GameObject purchaseManager;
    public GameObject sellManager;

    public TextMeshProUGUI money;

    public GameObject BuyPanel;
    public GameObject SellPanel;

    public GameObject BuyBtn;
    public GameObject SellBtn;

    public bool marketBuy;

    public bool isMarketPanelActive = false;

    private void Update()
    {
        // MarketPanel�� Ȱ��ȭ ���¸� Ȯ���Ͽ� �÷��� ����
        isMarketPanelActive = BuyPanel.activeSelf || SellPanel.activeSelf;

        if (isMarketPanelActive)
        {
            money.gameObject.SetActive(true);
        }
        else
        {
            money.gameObject.SetActive(false);
        }

        if (Inventory.instance.IsOpen())
        {
            SellBtn.SetActive(false);
            BuyBtn.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        // MarketPanel�� Ȱ��ȭ�Ǿ� �ִ� ���� Ŭ�� �̺�Ʈ�� ����
        if (isMarketPanelActive) return;

        // SellBtn�� BuyBtn�� Ȱ��ȭ
        SellBtn.SetActive(true);
        BuyBtn.SetActive(true);
        Inventory.instance.inventoryWindow.SetActive(false);
    }

    public void OnSellBtnClick()
    {
        Inventory.instance.inventoryWindow.SetActive(false);
        marketBuy = false;
        SellPanel.SetActive(true);
        SellBtn.SetActive(false);
        BuyBtn.SetActive(false);
    }

    public void OnBuyBtnClick()
    {
        Inventory.instance.inventoryWindow.SetActive(false);
        marketBuy = true;
        BuyPanel.SetActive(true);
        SellBtn.SetActive(false);
        BuyBtn.SetActive(false);
    }
}
