using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuButton : MonoBehaviour
{
    [SerializeField] private Image selectionImage;
    [SerializeField] private Color selectionColor;
    [SerializeField] private Color defaultColor;
    public GameObject menuToTurnOn;
    private bool selected;


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
