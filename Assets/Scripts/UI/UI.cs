using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI instance;
    
    private void Awake() 
    {
        if(!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    [Header("Inventory")]
    public Transform notesParent;
    public Transform keyItemsParent;
    public Transform charmsParent;
    public Transform equipedCharmsParent;
    public ItemDescriptionUI itemDescription;

    [Header("Game Menu")]
    [SerializeField] private GameObject gameMenu;
    [SerializeField] private GameMenuButton[] menuButtons;
    [SerializeField] private GameObject itemsTab;
    [SerializeField] private GameObject mapTab;
    [SerializeField] private GameObject charmsTab;
    [SerializeField] private GameObject notesTab;
    private int selectedIndex;

    private void Start() 
    {
        gameMenu.SetActive(false);    
    }

    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            gameMenu.SetActive(!gameMenu.activeSelf);    
            
            if(gameMenu.activeSelf)
                DefaultMenu();
        }

        if(gameMenu.activeSelf)
            NavigateTabs();
        
    }

    private void NavigateTabs()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            menuButtons[selectedIndex].Select(false);
            selectedIndex--;
            if (selectedIndex < 0)
                selectedIndex = menuButtons.Length - 1;

            menuButtons[selectedIndex].Select(true);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            menuButtons[selectedIndex].Select(false);
            selectedIndex = (selectedIndex + 1) % menuButtons.Length;
            menuButtons[selectedIndex].Select(true);
        }
    }

    private void DefaultMenu()
    {
        foreach (GameMenuButton button in menuButtons)
        {
            if (button == menuButtons[0])
                button.Select(true);
            else
                button.Select(false);
        }

        selectedIndex = 0;
    }

    public void EnableUI(bool enable)
    {
        if(!enable)
        {
            Time.timeScale = 0;

            Inventory.instance.UpdateSlotUI();
            GetComponentInChildren<ItemsUI>().SwitchTo();
        }
        else
            Time.timeScale = 1;
    }
}
