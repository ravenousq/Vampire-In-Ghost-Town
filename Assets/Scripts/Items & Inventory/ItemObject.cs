using UnityEngine;

[SelectionBase]
public class ItemObject : MonoBehaviour
{
    [SerializeField] private ItemData item;

    private void OnValidate() => gameObject.name = item != null ? item.itemName : "Item Object";

    public void SetUpItem(ItemData item) => this.item = item;

    public void PickUpItem()
    {
        Inventory.instance.AddItem(item);

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<Player>())
            PickUpItem();
    }
}
