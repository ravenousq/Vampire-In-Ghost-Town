using TMPro;
using UnityEngine;

public class SkillDisplay : ItemDisplay
{
    [SerializeField] protected TextMeshProUGUI priceText;

    public override void SetUp(string title, string description, string price)
    {
        base.SetUp(title, description);

        priceText.text = (price == null || price  == "0") ? "" : $"Price: {price}";
    }
}
