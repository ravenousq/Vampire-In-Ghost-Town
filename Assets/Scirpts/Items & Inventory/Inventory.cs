using System.Collections.Generic;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
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

    public List<InventoryItem> equipedCharms;
    public Dictionary<CharmData, InventoryItem> equipedCharmsDictionary;

    [Header("Inventory UI")]
    [SerializeField] private Canvas inventoryUI;
    [SerializeField] private Transform notesParent;
    [SerializeField] private Transform keyItemsParent;
    [SerializeField] private Transform charmsParent;
    [SerializeField] private Transform equipedCharmsParent;
    [Space]
    [SerializeField] private GameObject itemDisplayPrefab;
    [SerializeField] private List<RectTransform> itemDisplays;
    private ItemSlotUI[] notesSlots;
    private ItemSlotUI[] keyItemsSlots;
    private ItemSlotUI[] charmsSlots;
    private ItemSlotUI[] equipedCharmsSlots;

    private void Start() 
    {
        notes = new List<InventoryItem>();
        noteDictionary = new Dictionary<ItemData, InventoryItem>();

        keyItems = new List<InventoryItem>();
        keyItemsDictionary = new Dictionary<KeyItemData, InventoryItem>();

        charms = new List<InventoryItem>();
        charmsDictionary = new Dictionary<CharmData, InventoryItem>();

        equipedCharms = new List<InventoryItem>();
        equipedCharmsDictionary = new Dictionary<CharmData, InventoryItem>();

        notesSlots = notesParent.GetComponentsInChildren<ItemSlotUI>();
        keyItemsSlots = keyItemsParent.GetComponentsInChildren<ItemSlotUI>();
        charmsSlots = charmsParent.GetComponentsInChildren<ItemSlotUI>();
        equipedCharmsSlots = equipedCharmsParent.GetComponentsInChildren<ItemSlotUI>();
        
        itemDisplays = new List<RectTransform>();

        for (int i = 0; i < startingEquipment.Count; i++)
            AddItem(startingEquipment[i]);

    }

    public void EquipCharm(CharmData item)
    {
        if(!item)
            return;

        if(equipedCharmsDictionary.ContainsKey(item))
        {
            UnequipCharm(item);
            return;
        }
        
        for (int i = 0; i < equipedCharmsSlots.Length; i++)
        {
            if(equipedCharmsSlots[i].item == null)
                break;
            else if(i == equipedCharmsSlots.Length - 1)
                return;
        }

        InventoryItem newCharm = new InventoryItem(item);
        equipedCharms.Add(newCharm);
        equipedCharmsDictionary.Add(item, newCharm);
        item.EquipEffects();

        UpdateSlotUI();
    }

    public void UnequipCharm(CharmData item)
    {
        if(equipedCharmsDictionary.TryGetValue(item, out InventoryItem value))
        {
            equipedCharms.Remove(value);
            equipedCharmsDictionary.Remove(item);
            item.UnequipEffects();
        }

        UpdateSlotUI();
    }

    private void UpdateSlotUI()
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

        for (int i = 0; i < equipedCharms.Count; i++)
            equipedCharmsSlots[i].UpdateSlot(equipedCharms[i]);
    }

    public void AddItem(ItemData item)
    {
        if (item.itemType == ItemType.Note)
            AddToNotes(item);


        if (item.itemType == ItemType.KeyItem)
            AddToKeyItems(item as KeyItemData);


        if (item.itemType == ItemType.Charm)
            AddToCharms(item as CharmData);

        DisplayItem(item);

        UpdateSlotUI();
    }

    private void DisplayItem(ItemData item)
    {
        GameObject newItemDisplay = Instantiate(itemDisplayPrefab);
        ItemDisplayUI newUI = newItemDisplay.GetComponentInChildren<ItemDisplayUI>();

        newUI.SetUp(item);

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

        UpdateSlotUI();
    }

    private void AddToNotes(ItemData item)
    {
        Debug.Log("Adding note");
        if(noteDictionary.TryGetValue(item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(item);
            notes.Add(newItem);
            noteDictionary.Add(item, newItem);
        }
    }

    private void AddToKeyItems(KeyItemData item)
    {
        Debug.Log(item.description);
        if(keyItemsDictionary.TryGetValue(item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(item);
            keyItems.Add(newItem);
            keyItemsDictionary.Add(item, newItem);
        }

        UpdateSlotUI();
    }

    private void AddToCharms(CharmData item)
    {
        if(charmsDictionary.TryGetValue(item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(item);
            charms.Add(newItem);
            charmsDictionary.Add(item, newItem);
        }
    }

}
