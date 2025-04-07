using UnityEngine;
using UnityEngine.UI;

public class InventoryItemControler : MonoBehaviour
{
    InventoryObject item;

    public Button RemoveButton;

    public void RemoveItem()
    {
        InventoryManager.instance.Remove(item);

        Destroy(gameObject);
    }

    public void AddItem(InventoryObject newItem)
    {
        item = newItem;
    }

    public void UseItem()
    {
        switch (item.itemType)
        {
            case InventoryObject.ItemType.Llave:
                break;
        }

        RemoveItem();
    }
}
