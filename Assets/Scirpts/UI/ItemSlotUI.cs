using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ItemSlotUI : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;

    public InventoryItem item;

    public void UpdateSlot(InventoryItem item)
    {
        this.item = item;
        itemImage.color = Color.white;

        if(item != null)
        {
            itemImage.sprite = item.itemData.icon;
            itemText.text = item.stackSize > 1 ? item.stackSize.ToString() : "";
        }
    }


    public void CleanUpSlot()
    {
        item = null;
        itemImage.sprite = null;
        itemImage.color = Color.clear;
        itemText.text = "";
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if(item == null || !item.itemData)
            return;
            
        Debug.Log(item.itemData.description);

        if(item.itemData.itemType == ItemType.Charm)
            Inventory.instance.EquipCharm(item.itemData as CharmData);
    }
}
