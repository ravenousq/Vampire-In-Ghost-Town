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
    [SerializeField] private GameObject blessingsTab;
    [SerializeField] private GameObject notesTab;
    [Space]
    [SerializeField] private GameObject InGameUI;
    [SerializeField] private SoulsUI inGameSoulsUI;
    private int selectedIndex;

    private void Start() 
    {
        blessingsTab.GetComponent<BlessingsUI>().TabSwitch();
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
            if(selectedIndex == 4)
                notesTab.GetComponent<NotesUI>().Reset();
            else if(selectedIndex == 2)
                charmsTab.GetComponent<CharmsUI>().TabSwitch();

            menuButtons[selectedIndex].Select(false);
            selectedIndex--;

            if (selectedIndex < 0)
                selectedIndex = menuButtons.Length - 1;

            if(selectedIndex  == 3)
                blessingsTab.GetComponentInChildren<BlessingsUI>(true).TabSwitch();

            menuButtons[selectedIndex].Select(true);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if(selectedIndex == 4)
                notesTab.GetComponent<NotesUI>().Reset();
            else if(selectedIndex == 2)
                charmsTab.GetComponent<CharmsUI>().TabSwitch();
            else if(selectedIndex + 1 == 3)
                blessingsTab.GetComponentInChildren<BlessingsUI>(true).TabSwitch();

            menuButtons[selectedIndex].Select(false);
            selectedIndex = (selectedIndex + 1) % menuButtons.Length;

            if(selectedIndex  == 3)
                blessingsTab.GetComponentInChildren<BlessingsUI>(true).TabSwitch();

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
            GetComponentInChildren<ItemsUI>()?.SwitchTo();
        }
        else
        {
            GetComponentInChildren<NotesUI>()?.Reset();
            GetComponentInChildren<BlessingsUI>()?.TabSwitch();
            GetComponentInChildren<CharmsUI>()?.TabSwitch();
            Time.timeScale = 1;
        }
    }

    public void UnlockSecretSkill(string name) => GetComponentInChildren<BlessingsUI>(true).UnlockSecretSkill(name);
    public void ModifySouls(int souls) => GetComponentInChildren<SoulsUI>(true).ModifySouls(souls);
    public void UpdateInGameSouls() => inGameSoulsUI.UpdateSouls();
}
