using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData itemData;   

    public void Pickup()
    {
        bool added = InventoryManager.Instance.AddItem(itemData);
        if (added)
        {
            Destroy(gameObject);
        }
    }
}
