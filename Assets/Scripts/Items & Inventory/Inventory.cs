using System.Collections.Generic;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

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

    public List<ItemData> startingEquipment;

    public List<InventoryItem> notes;
    public Dictionary<ItemData, InventoryItem> noteDictionary;

    public List<InventoryItem> keyItems;
    public Dictionary<KeyItemData, InventoryItem> keyItemsDictionary;

    public List<InventoryItem> charms;
    public Dictionary<CharmData, InventoryItem> charmsDictionary;

    public InventoryItem[] equipedCharms;
    public Dictionary<CharmData, InventoryItem> equipedCharmsDictionary;

    [Header("Inventory UI")]
    [SerializeField] private GameObject itemDisplayPrefab;
    [SerializeField] private List<RectTransform> itemDisplays;
    private ItemSlotUI[] notesSlots;
    private ItemSlotUI[] keyItemsSlots;
    private ItemSlotUI[] charmsSlots;
    private ItemSlotUI[] equipedCharmsSlots;
    private UI ui;


    private void Start() 
    {
        ui = UI.instance;

        notes = new List<InventoryItem>();
        noteDictionary = new Dictionary<ItemData, InventoryItem>();

        keyItems = new List<InventoryItem>();
        keyItemsDictionary = new Dictionary<KeyItemData, InventoryItem>();

        charms = new List<InventoryItem>();
        charmsDictionary = new Dictionary<CharmData, InventoryItem>();

        equipedCharms = new InventoryItem[5];
        equipedCharmsDictionary = new Dictionary<CharmData, InventoryItem>();

        notesSlots = ui.notesParent.GetComponentsInChildren<ItemSlotUI>();
        keyItemsSlots = ui.keyItemsParent.GetComponentsInChildren<ItemSlotUI>();
        charmsSlots = ui.charmsParent.GetComponentsInChildren<ItemSlotUI>();
        equipedCharmsSlots = ui.equipedCharmsParent.GetComponentsInChildren<ItemSlotUI>();
        
        itemDisplays = new List<RectTransform>();

        for (int i = 0; i < startingEquipment.Count; i++)
            AddItem(startingEquipment[i]);

    }

    public void EquipCharm(CharmData item, int slotToEquip = -1)
    {
        if(!item)
            return;

        if(equipedCharmsDictionary.ContainsKey(item))
        {
            for (int i = 0; i < equipedCharms.Length; i++)
            {
                if(equipedCharms[i]?.itemData == item)
                {
                    UnequipCharm(item, i);
                    return;
                }
            }
        }

        InventoryItem newCharm = new InventoryItem(item);
        if(equipedCharms[slotToEquip] != null)
            UnequipCharm(equipedCharms[slotToEquip].itemData as CharmData, slotToEquip);
        equipedCharms[slotToEquip] = newCharm;
        equipedCharmsDictionary.Add(item, newCharm);
        item.EquipEffects();

        UpdateSlotUI();
    }

    public void UnequipCharm(CharmData item, int slotToUnequip = -1)
    {
        if(equipedCharmsDictionary.TryGetValue(item, out InventoryItem value))
        {
            equipedCharms[slotToUnequip] = null;
            equipedCharmsDictionary.Remove(item);
            item.UnequipEffects();
        }

        UpdateSlotUI();
    }

    public void UpdateSlotUI()
    {
        for (int i = 0; i < notesSlots.Length; i++)
            notesSlots[i].CleanUpSlot();

        for (int i = 0; i < keyItemsSlots.Length; i++)
            keyItemsSlots[i].CleanUpSlot();

        for (int i = 0; i < charmsSlots.Length; i++)
            charmsSlots[i].CleanUpSlot();
        
        for (int i = 0; i < equipedCharmsSlots.Length; i++)
            equipedCharmsSlots[i].CleanUpSlot();


        for (int i = 0; i < notes.Count; i++)
            notesSlots[i].UpdateSlot(notes[i]);

        for (int i = 0; i < keyItems.Count; i++)
            keyItemsSlots[i].UpdateSlot(keyItems[i]);

        for (int i = 0; i < charms.Count; i++)
            charmsSlots[i].UpdateSlot(charms[i]);

        for (int i = 0; i < equipedCharms.Length; i++)
            equipedCharmsSlots[i].UpdateSlot(equipedCharms[i]);
    }

    public void AddItem(ItemData item, bool confirmation = false)
    {
        if (item.itemType == ItemType.Note)
            AddToNotes(item);

        if (item.itemType == ItemType.KeyItem)
            AddToKeyItems(item as KeyItemData);

        if (item.itemType == ItemType.Charm)
            AddToCharms(item as CharmData);

        DisplayItem(item, confirmation);

        //UpdateSlotUI();
    }

    public bool HasItem(ItemData item)
    {
        if(item.itemType == ItemType.KeyItem && keyItemsDictionary.ContainsKey(item as KeyItemData))
            return true;
        
        if(item.itemType == ItemType.Charm &&charmsDictionary.ContainsKey(item as CharmData))
            return true;

        if(item.itemType == ItemType.Note &&noteDictionary.ContainsKey(item))
            return true;

        return false;
    }

    private void DisplayItem(ItemData item, bool confirmation)
    {
        GameObject newItemDisplay = Instantiate(itemDisplayPrefab);
        ItemDisplayUI newUI = newItemDisplay.GetComponentInChildren<ItemDisplayUI>();

        newUI.SetUp(item, confirmation);

        itemDisplays.RemoveAll(item => item == null);

        foreach (RectTransform display in itemDisplays)
            display.anchoredPosition += new Vector2(0, 250);

        itemDisplays.Add(newUI.GetComponent<RectTransform>());
    }

    public void RemoveItem(ItemData item)
    {
        if(keyItemsDictionary.TryGetValue(item as KeyItemData, out InventoryItem keyItemValue))
        {
            if(keyItemValue.stackSize <= 1)
            {
                keyItems.Remove(keyItemValue);
                keyItemsDictionary.Remove(item as KeyItemData);
            }
            else
                keyItemValue.RemoveStack();
        }

        if(charmsDictionary.TryGetValue(item as CharmData, out InventoryItem charmValue))
        {
            if(charmValue.stackSize <= 1)
            {
                charms.Remove(charmValue);
                charmsDictionary.Remove(item as CharmData);
            }
            else
                charmValue.RemoveStack();
        }

        //UpdateSlotUI();
    }

    private void AddToNotes(ItemData item)
    {
        if(noteDictionary.TryGetValue(item, out InventoryItem value))
            value.AddStack();
        else
        {
            InventoryItem newItem = new InventoryItem(item);
            notes.Add(newItem);
            noteDictionary.Add(item, newItem);
        }
    }

    private void AddToKeyItems(KeyItemData item)
    {
        if(keyItemsDictionary.TryGetValue(item, out InventoryItem value))
            value.AddStack();
        else
        {
            InventoryItem newItem = new InventoryItem(item);
            keyItems.Add(newItem);
            keyItemsDictionary.Add(item, newItem);
        }

        item.PickUpEffect();

        //UpdateSlotUI();
    }

    private void AddToCharms(CharmData item)
    {
        if(charmsDictionary.TryGetValue(item, out InventoryItem value))
            value.AddStack();
        else
        {
            InventoryItem newItem = new InventoryItem(item);
            charms.Add(newItem);
            charmsDictionary.Add(item, newItem);
        }
    }

}
