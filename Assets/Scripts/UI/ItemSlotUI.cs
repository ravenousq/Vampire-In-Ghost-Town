using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ItemSlotUI : MonoBehaviour
{
    [SerializeField] private Image frame;
    [SerializeField] private Image itemImage;
    [SerializeField] private Image selectedImage;
    [SerializeField] private TextMeshProUGUI itemText;
    private ItemDescriptionUI itemDescription;

    public InventoryItem item;

    private void Start()
    {
        itemDescription = UI.instance.itemDescription;
        if(gameObject.name.Contains("Equipped Charm Slot"))
           frame.color = Color.white;
    }

    public void UpdateSlot(InventoryItem item)
    {
        if(item == null)
            return;

        this.item = item;
        itemImage.color = Color.white;

        if(gameObject.name.Contains("Equipped Charm Slot") || item != null)
           frame.color = Color.white;
         
        if(item != null && item.itemData != null)
        {
            itemImage.sprite = item?.itemData.icon;
            itemText.text = item.stackSize > 1 ? item.stackSize.ToString() : "";
        }
    }


    public void CleanUpSlot()
    {
        // if(!gameObject.name.Contains("Equipped Charm Slot"))
        //    frame.color = Color.clear;
        item = null;
        itemImage.sprite = null;
        itemImage.color = Color.clear;
        itemText.text = "";

    }

    public void AchorDescription(float xPadding = 500, float yPadding = 175)
    {
        RectTransform thisRect = GetComponent<RectTransform>();
        RectTransform descRect = itemDescription.GetComponent<RectTransform>();
        Canvas canvas = UI.instance.GetComponent<Canvas>();

        Vector3[] corners = new Vector3[4];
        thisRect.GetWorldCorners(corners);

        Vector3 worldTarget = (corners[0] + corners[1]) * 0.5f;

        worldTarget.x -= xPadding / canvas.scaleFactor;
        worldTarget.y -= yPadding / canvas.scaleFactor;

        descRect.position = worldTarget;
    }

    public void Select(bool selected)
    {
        selectedImage.color = selected ? Color.white : Color.clear;
    }
}
