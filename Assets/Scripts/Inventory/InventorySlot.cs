using UnityEngine;
using UnityEngine.UI;

/// Attached to the InventorySlot prefab.
/// Displays one item and notifies ItemSelectionState when clicked.
[RequireComponent(typeof(Button))]
public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image iconImage;

    private ItemData _item;

    public void Setup(ItemData item)
    {
        _item = item;
        iconImage.sprite = item.icon;
        iconImage.enabled = item.icon != null;
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(OnClicked);
    }

    private void OnClicked()
    {
        ItemSelectionState.Instance.SelectItem(_item);
    }
}