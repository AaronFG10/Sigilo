using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item / Create New Item")]
public class InventoryObject : ScriptableObject
{
    public int id;
    public string itemName;
    public int value;
    public Sprite icon;
    public ItemType itemType;
    public GameObject prefab;

    public enum ItemType
    {
        Llave,
    }
}
