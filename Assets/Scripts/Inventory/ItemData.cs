using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemId;       
    public string displayName;
    public Sprite icon;         
    [TextArea] public string description;
}
