using TMPro;
using UnityEngine;

public class ItemDisplay : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI title;
    [SerializeField] protected TextMeshProUGUI description;

    public void SetUp(ItemData itemData)
    {
        if(itemData == null)
        {
            title.text = "";
            description.text = "";
            return;
        }

        title.text = itemData.itemName;
        description.text = itemData.itemDescription;
    }

    public virtual void SetUp(string title, string description, string price = "")
    {
        this.title.text = title;
        this.description.text = description;
    }
}
