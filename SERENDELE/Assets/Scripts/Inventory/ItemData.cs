using System;
using UnityEngine;

public enum ItemType
{
    Resource,   // �ڿ�(����, �� ��)
    Equipable,  // ������ �� ����(����, �� ��)
    Consumable  // ����, ����
}
public enum EquipableType
{
    Head,    
    Body,
    Shoes,
    Weapon,
    None
}

public enum AttackDefenseType
{
    Offense,
    Defense
}

[Serializable]
public class ItemDataConsumable
{
    public float value;
}

[Serializable]
public class ItemDataAttackDefense
{
    public AttackDefenseType type;
    public float value;
}

[Serializable]
public class SerializableItemData
{
    public string displayName;    // �̸�
    public string description;    // ����
    public ItemType type;         // ������ Ÿ��
    public EquipableType equipableType;  // ���� Ÿ��
    public int quantity;          // ����
    public bool canStack;         // ���� �� ���� ����
    public int maxStackAmount;    // �ִ� ���� ����
    public ItemDataAttackDefense[] attackDefense;  // ���ݷ�, ����
    public int consumableValue;   // �Ҹ� ������ �������� ü�°� ����� ������
    public int itemPrice;         // ����
    public string dropPrefabPath; // Prefab�� ��θ� ����
    public string iconPath;       // �������� ��θ� ����
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;    // �̸�
    public string description;    // ����
    public ItemType type;         // ������ Ÿ��
    public EquipableType equipableType;         // ���� Ÿ��
    public Sprite icon;           // �κ��丮 ������
    public GameObject dropPrefab; // ������
    public int quantity;      // ����(���� �����ϴ°� �ƴ϶� Firebase���� �ʿ�)

    [Header("Stacking")]
    public bool canStack;       // ���� �� ���� ����
    public int maxStackAmount;  // �ִ� ���� ����

    // ����, ���� ���ݷ�, ����
    [Header("OffenseDefense")]
    public ItemDataAttackDefense[] attackDefense;

    // �Ҹ� ������ �������� ü�°� ����� ������
    [Header("Consumable")]
    public int consumableValue;

    // ����
    [Header("Price")]
    public int itemPrice;

    // SerializableItemData�� ��ȯ�ϴ� �޼���
    public SerializableItemData ToSerializable()
    {
        return new SerializableItemData
        {
            displayName = displayName,
            description = description,
            type = type,
            equipableType = equipableType,
            quantity = quantity,
            canStack = canStack,
            maxStackAmount = maxStackAmount,
            attackDefense = attackDefense,
            consumableValue = consumableValue,
            itemPrice = itemPrice,
            dropPrefabPath = dropPrefab != null ? "Item/" + dropPrefab.name : null,
            iconPath = icon != null ? "Icons/"+ icon.name : null
        };
    }

    // SerializableItemData���� ItemData�� ��ȯ�ϴ� �޼���
    public void FromSerializable(SerializableItemData serializableItemData)
    {
        displayName = serializableItemData.displayName;
        description = serializableItemData.description;
        type = serializableItemData.type;
        equipableType = serializableItemData.equipableType;
        quantity = serializableItemData.quantity;
        canStack = serializableItemData.canStack;
        maxStackAmount = serializableItemData.maxStackAmount;
        attackDefense = serializableItemData.attackDefense;
        consumableValue = serializableItemData.consumableValue;
        itemPrice = serializableItemData.itemPrice;

        // Resources �������� Prefab �ε�
        if (!string.IsNullOrEmpty(serializableItemData.dropPrefabPath))
        {
            dropPrefab = Resources.Load<GameObject>(serializableItemData.dropPrefabPath);
        }
        else
        {
            dropPrefab = null;
        }

        // ������ �ε�
        if (!string.IsNullOrEmpty(serializableItemData.iconPath))
        {
            icon = Resources.Load<Sprite>(serializableItemData.iconPath);
        }
        else
        {
            icon = null;
        }
    }
}
