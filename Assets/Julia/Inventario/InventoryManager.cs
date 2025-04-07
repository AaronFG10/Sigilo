using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public List<InventoryObject> items = new List<InventoryObject>();

    public Transform ItemContent;
    public GameObject InventoryItem;

    public Toggle enableRemove;

    public InventoryItemControler[] inventoryItems;

    private void Awake()
    {
        instance = this;
    }

    public void Add(InventoryObject item)
    {
        items.Add(item);
    }

    public void Remove(InventoryObject item)
    {
        items.Remove(item);
    }

    public void ListItems()
    {
        foreach (Transform item in ItemContent)
        {
            Destroy (item.gameObject);
        }

        foreach (var item in items)
        {
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
            var itemName = obj.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
            var removeButton = obj.transform.Find("RemoveButton").GetComponent<Button>();

            itemIcon.sprite = item.icon;
            itemName.text = item.itemName;

            if (enableRemove.isOn)
            {
                removeButton.gameObject.SetActive(true);
            }
        }

        SetInventoryItems();
    }

    public void EnableItemsRemove()
    {
        if (enableRemove.isOn)
        {
            foreach (Transform item in ItemContent)
            {
                item.Find("RemoveButton").gameObject.SetActive(true);
            }
        }

        else
        {
            foreach (Transform item in ItemContent)
            {
                item.Find("RemoveButton").gameObject.SetActive(true);
            }
        }
    }

    public void SetInventoryItems()
    {
        inventoryItems = ItemContent.GetComponentsInChildren<InventoryItemControler>();

        for (int i = 0; i < items.Count; i++)
        {
            inventoryItems[i].AddItem(items[i]);
        }
    }
}
