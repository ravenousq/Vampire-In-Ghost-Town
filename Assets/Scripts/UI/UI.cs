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

    [Header("NPC Shop UI")]
    [SerializeField] private NPCShopUI npcShop;

    [Header("Game Menu")]
    [SerializeField] private GameObject gameMenu;
    [SerializeField] private GameMenuButton[] menuButtons;
    [SerializeField] private ItemsUI itemsTab;
    [SerializeField] private GameObject mapTab;
    [SerializeField] private CharmsUI charmsTab;
    [SerializeField] private BlessingsUI blessingsTab;
    [SerializeField] private NotesUI notesTab;
    [Space]
    [SerializeField] private GameObject InGameUI;
    [SerializeField] private SoulsUI inGameSoulsUI;
    
    [Header("Debug")]
    [SerializeField] private NPC amelia;
    private int selectedIndex;

    private void Start() 
    {
        blessingsTab.TabSwitch();
        for (int i = 0; i < menuButtons.Length; i++)
        {
            menuButtons[i].Select(true);
            menuButtons[i].Select(false);
            
        }
        gameMenu.SetActive(false);
    }

    private void Update() 
    {

        if (Input.GetKeyDown(KeyCode.P))
        {
            gameMenu.SetActive(!gameMenu.activeSelf);    
            
            if(gameMenu.activeSelf)
                DefaultMenu();
        }

        if(gameMenu.activeSelf)
            NavigateTabs();
        
    }

    public void SwitchShop(NPC npc, int index)
    {
        Time.timeScale = !npcShop.gameObject.activeSelf ? 0 : 1;
        EnableNPCShop(!npcShop.gameObject.activeSelf);
        npcShop.SetUp(npc, index);
    }

    private void NavigateTabs()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(selectedIndex == 4)
                notesTab.Reset();
            else if(selectedIndex == 2)
                charmsTab.TabSwitch();

            menuButtons[selectedIndex].Select(false);
            selectedIndex--;

            if (selectedIndex < 0)
                selectedIndex = menuButtons.Length - 1;

            if(selectedIndex  == 3)
                blessingsTab.TabSwitch();

            menuButtons[selectedIndex].Select(true);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if(selectedIndex == 4)
                notesTab.Reset();
            else if(selectedIndex == 2)
                charmsTab.TabSwitch();
            else if(selectedIndex + 1 == 3)
                blessingsTab.TabSwitch();

            menuButtons[selectedIndex].Select(false);
            selectedIndex = (selectedIndex + 1) % menuButtons.Length;

            if(selectedIndex  == 3)
                blessingsTab.TabSwitch();

            menuButtons[selectedIndex].Select(true);
        }
    }

    public void EnableNPCShop(bool enable)
    {
        npcShop.gameObject.SetActive(enable);
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
            itemsTab.SwitchTo();
        }
        else
        {
            notesTab.Reset();
            blessingsTab.TabSwitch();
            charmsTab.TabSwitch();
            Time.timeScale = 1;
        }
    }

    public void UnlockSecretSkill(string name) => blessingsTab.UnlockSecretSkill(name);
    public void ModifySouls(int souls) => GetComponentInChildren<SoulsUI>(true).ModifySouls(souls);
    public void UpdateInGameSouls() => inGameSoulsUI.UpdateSouls();
    public void UpdateSkillsSouls() => blessingsTab.UpdateSouls();
}
