using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Auth;
using System.Collections;
using System;

public class FirebaseManager : MonoBehaviour
{
    private DatabaseReference databaseReference;
    private bool isInitialized = false;
    private Queue<ItemData> itemDataQueue = new Queue<ItemData>();
    private FirebaseAuth auth;
    private FirebaseUser user;

    void Start()
    {
        // Firebase �ʱ�ȭ
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
            auth = FirebaseAuth.DefaultInstance;

            // �̸��ϰ� ��й�ȣ�� �α���
            string email = "user@example.com"; // ���� ���� �̸���
            string password = "userpassword"; // ���� ���� ��й�ȣ

            auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(authTask => {
                if (authTask.IsCompleted)
                {
                    user = auth.CurrentUser;
                    isInitialized = true;

                    // ť�� �ִ� ������ ����
                    while (itemDataQueue.Count > 0)
                    {
                        SaveItemData(itemDataQueue.Dequeue());
                    }
                }
                else
                {
                    Debug.LogError("Firebase Authentication failed: " + authTask.Exception);
                }
            });
        });
    }

    // �̸����� Firebase-friendly ���ڿ��� ��ȯ
    private string GetFirebaseKeyFromEmail(string email)
    {
        return email.Replace(".", ",");
    }

    // ������ ������ ���� (�����)
    public void SaveItemData(ItemData itemData)
    {
        SaveDataToFirebase("Inventory", itemData);
    }

    public void SaveStorageData(ItemData itemData)
    {
        SaveDataToFirebase("Storage", itemData);
    }

    private void SaveDataToFirebase(string category, ItemData itemData)
    {
        if (!isInitialized)
        {
            Debug.LogWarning("Firebase is not initialized yet. Queueing the data to save later.");
            itemDataQueue.Enqueue(itemData);
            return;
        }

        string userEmailKey = GetFirebaseKeyFromEmail(user.Email);
        string itemKey = itemData.displayName;
        SerializableItemData serializableData = itemData.ToSerializable();
        string jsonData = JsonUtility.ToJson(serializableData);

        databaseReference.Child($"{category}").Child($"{userEmailKey}").Child(itemKey).SetRawJsonValueAsync(jsonData).ContinueWithOnMainThread(task => {
            if (task.IsCompleted)
            {
                Debug.Log($"{category}-{userEmailKey} item data saved successfully.");
            }
            else
            {
                Debug.LogError($"Failed to save {category}-{userEmailKey} item data: " + task.Exception);
            }
        });
    }

    // ���� ������ ������ �ҷ�����
    public void LoadUserItems(System.Action<List<ItemData>> callback)
    {
        LoadDataFromFirebase("Inventory", callback);
    }

    public void LoadStorageItems(System.Action<List<ItemData>> callback)
    {
        LoadDataFromFirebase("Storage", callback);
    }

    private void LoadDataFromFirebase(string category, System.Action<List<ItemData>> callback)
    {
        if (!isInitialized)
        {
            return;
        }

        string userEmailKey = GetFirebaseKeyFromEmail(user.Email);

        databaseReference.Child($"{category}").Child($"{userEmailKey}").GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                List<ItemData> itemList = new List<ItemData>();

                foreach (DataSnapshot itemSnapshot in snapshot.Children)
                {
                    try
                    {
                        string json = itemSnapshot.GetRawJsonValue();
                        SerializableItemData serializableItemData = JsonUtility.FromJson<SerializableItemData>(json);
                        ItemData itemData = ScriptableObject.CreateInstance<ItemData>();
                        itemData.FromSerializable(serializableItemData);
                        itemList.Add(itemData);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Error parsing item data: " + e);
                    }
                }

                Debug.Log($"{category}-{userEmailKey} Loaded items count: " + itemList.Count);
                callback(itemList);
            }
            else
            {
                Debug.LogError($"Failed to load {category}-{userEmailKey} user items: " + task.Exception);
            }
        });
    }

    // ������ ������ ����
    public void DeleteItemData(ItemData itemData)
    {
        DeleteDataFromFirebase("Inventory", itemData);
    }

    public void DeleteStorageData(ItemData itemData)
    {
        DeleteDataFromFirebase("Storage", itemData);
    }

    private void DeleteDataFromFirebase(string category, ItemData itemData)
    {
        if (itemData == null)
        {
            Debug.LogError("itemData is null. Cannot delete item data.");
            return;
        }
        else
        {
            Debug.Log(itemData.ToString());
        }

        if (string.IsNullOrEmpty(itemData.displayName))
        {
            Debug.LogError("itemData.displayName is null or empty. Cannot delete item data.");
            return;
        }
        else
        {
            Debug.Log(itemData.displayName);
        }

        if (!isInitialized)
        {
            Debug.LogWarning("Firebase is not initialized yet. Cannot delete item data.");
            return;
        }

        string userEmailKey = GetFirebaseKeyFromEmail(user.Email);
        string itemKey = itemData.displayName;

        databaseReference.Child($"{category}").Child($"{userEmailKey}").Child(itemKey).RemoveValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsCompleted)
            {
                Debug.Log($"{category}-{userEmailKey} item data deleted successfully.");
            }
            else
            {
                Debug.LogError($"Failed to delete {category}-{userEmailKey} item data: " + task.Exception);
            }
        });
    }
}
