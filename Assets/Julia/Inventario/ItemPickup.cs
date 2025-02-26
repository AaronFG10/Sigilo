using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public InventoryObject item;

    void Pickup()
    {
        InventoryManager.instance.Add(item);
        Destroy (gameObject);
    }

    private void OnMouseDown()
    {
        Pickup();
    }
}
