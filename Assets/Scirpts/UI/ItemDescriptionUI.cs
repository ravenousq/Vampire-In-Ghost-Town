using TMPro;
using UnityEngine;

public class ItemDescriptionUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    private ItemData item;

    public void SetUp(string title, string description)
    {
        itemName.text = title;
        itemDescription.text = description;
    }
}
