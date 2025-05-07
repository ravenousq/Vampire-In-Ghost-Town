using TMPro;
using UnityEngine;

public class ItemDisplay : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI itemName;
    [SerializeField] protected TextMeshProUGUI itemDescription;

    public void SetUp(ItemData itemData)
    {
        if(itemData == null)
        {
            itemName.text = "";
            itemDescription.text = "";
            return;
        }

        itemName.text = itemData.itemName;
        itemDescription.text = itemData.itemDescription;
    }
}
