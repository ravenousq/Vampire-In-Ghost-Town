using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuButton : MonoBehaviour
{
    [SerializeField] private Image selectionImage;
    [SerializeField] private Color selectionColor;
    public GameObject menuToTurnOn;
    private bool selected;
    private Color defaultColor;

    private void Start() 
    {
        defaultColor = GetComponentInChildren<TextMeshProUGUI>().color;    
    }


    public void Select(bool selected)
    {
        this.selected = selected;

        GetComponent<Image>().color = selected ? Color.white : Color.clear;
        GetComponentInChildren<TextMeshProUGUI>().color = selected ? selectionColor : defaultColor;

        if(selected)
        {
            menuToTurnOn.SetActive(true);
            menuToTurnOn.GetComponent<ItemsUI>()?.SwitchTo();
        }
        else
            menuToTurnOn.SetActive(false);
    }
}
