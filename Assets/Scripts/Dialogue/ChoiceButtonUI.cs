using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class ChoiceButtonUI : MonoBehaviour
{
    public TextMeshProUGUI myText {get; private set; }
    public Image[] images;
    public bool canBeSelected = true;

    private void Awake() 
    {
        myText = GetComponentInChildren<TextMeshProUGUI>();    
        
        images = GetComponentsInChildren<Image>();
        foreach(Image image in images)
            image.color = Color.clear;
    }

    private ItemData requiredItem;
    private bool highlighted;
    public bool goToShop { get; private set; }
    private Color originalColor;

    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.C) && highlighted)
        {
            Debug.Log("Chosen " + myText.text);
        }
    }

    public void SetUp(string newText, ItemData requiredItem = null, bool shop = false)
    {
        originalColor = myText.color;     
        myText.text = newText;

        this.requiredItem = requiredItem;

        if(requiredItem != null)
        {
            images[0].sprite = requiredItem.icon;

            if(!Inventory.instance.HasItem(requiredItem))
            {
                myText.text = "";
                canBeSelected = false;
            }
            else
            {
                foreach(Image image in images)
                    image.color = Color.white;
            }
        }

        goToShop = shop;
    }

    public void Highlight()
    {
        highlighted = !highlighted;

        if(highlighted)
        {
            myText.fontSize *= 1.2f;
            myText.color = Color.white;
        }
        else
        {
            myText.fontSize /= 1.2f;
            myText.color = originalColor;
        }

        if(requiredItem)
        {
            foreach(Image image in images)
                image.GetComponent<RectTransform>().localScale *= highlighted ? 1.2f : 1/1.2f;
        }
    }
}
