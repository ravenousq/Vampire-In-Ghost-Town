using UnityEngine;

public enum ItemType
{
    Note,
    KeyItem,
    Charm,
}

[CreateAssetMenu(fileName = "New Note", menuName = "Data/Note")]
public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite icon;
    [TextArea(1, 10)] 
    public string itemDescription;
    [TextArea]
    public string noteContent;
}
