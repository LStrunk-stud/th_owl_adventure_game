using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item Data")]
public class ItemData : ScriptableObject
{
    [Header("Identity")]
    public string itemName;
    [TextArea] public string description;

    [Header("Visuals")]
    public Sprite icon;

    [Header("Settings")]
    public bool isBackpack;
}