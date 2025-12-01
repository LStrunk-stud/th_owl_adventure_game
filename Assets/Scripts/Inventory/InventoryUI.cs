using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public GameObject slotPrefab;         
    public Transform slotsParent;         
    public int slotCount = 8;

    private List<Image> slotImages = new List<Image>();

    void Start()
    {
        for (int i = 0; i < slotCount; i++)
        {
            GameObject go = Instantiate(slotPrefab, slotsParent);
            go.name = "Slot_" + i;
            Image icon = go.GetComponent<Image>(); 
            if (icon == null) icon = go.GetComponentInChildren<Image>();
            slotImages.Add(icon);
            
            if (InventoryManager.Instance != null && InventoryManager.Instance.emptySlotSprite != null)
                icon.sprite = InventoryManager.Instance.emptySlotSprite;
        }

        InventoryManager.Instance.OnInventoryChanged += Refresh;
    }

    void Refresh()
    {
        var items = InventoryManager.Instance.GetItems();

        for (int i = 0; i < slotCount; i++)
        {
            if (i < items.Count)
                slotImages[i].sprite = items[i].icon;
            // else
            //     slotImages[i].sprite = InventoryManager.Instance.emptySlotSprite;
            
            slotImages[i].color = (i < items.Count) ? Color.white : new Color(1,1,1,0.4f);
        }
    }

    private void OnDestroy()
    {
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.OnInventoryChanged -= Refresh;
    }
}
