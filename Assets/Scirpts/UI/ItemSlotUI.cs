using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ItemSlotUI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image frame;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;
    private ItemDescriptionUI itemDescription;

    public InventoryItem item;

    private void Start()
    {
        itemDescription = Inventory.instance.itemDescription;
        if(gameObject.name.Contains("Equipped Charm Slot"))
           frame.color = Color.white;
    }

    public void UpdateSlot(InventoryItem item)
    {
        this.item = item;
        itemImage.color = Color.white;

        if(gameObject.name.Contains("Equipped Charm Slot") || item != null)
           frame.color = Color.white;
         
        if(item != null)
        {
            itemImage.sprite = item.itemData.icon;
            itemText.text = item.stackSize > 1 ? item.stackSize.ToString() : "";
        }
    }


    public void CleanUpSlot()
    {
        if(!gameObject.name.Contains("Equipped Charm Slot"))
           frame.color = Color.clear;
        item = null;
        itemImage.sprite = null;
        itemImage.color = Color.clear;
        itemText.text = "";

    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if(item == null || !item.itemData)
            return;

        if(item.itemData.itemType == ItemType.Charm)
            Inventory.instance.EquipCharm(item.itemData as CharmData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(item == null || !item.itemData || gameObject.name.Contains("Equipped Charm Slot"))
            return;

        itemDescription.gameObject.SetActive(true);
        itemDescription.SetUp(item.itemData.itemName, item.itemData.description);
        AchorDescription();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(item == null || !item.itemData)
            return;

        itemDescription.gameObject.SetActive(false);
    }

    public void AchorDescription(float xPadding = 500, float yPadding = 175)
    {
        RectTransform thisRect = GetComponent<RectTransform>();
        RectTransform descRect = itemDescription.GetComponent<RectTransform>();
        Canvas canvas = Inventory.instance.canvas;

        Vector3[] corners = new Vector3[4];
        thisRect.GetWorldCorners(corners);

        Vector3 worldTarget = (corners[0] + corners[1]) * 0.5f;

        worldTarget.x -= xPadding / canvas.scaleFactor;
        worldTarget.y -= yPadding / canvas.scaleFactor;

        descRect.position = worldTarget;
    }

}
