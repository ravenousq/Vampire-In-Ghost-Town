using TMPro;
using UnityEngine;

public class KeyItemDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;

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
