using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCShopUI : ItemsUI
{
    [SerializeField] private TextMeshProUGUI npcName;
    private List<ItemData> stock;

    protected override void Awake()
    {
        base.Awake();

        stock = new List<ItemData>();
    }

    public void SetUp(string npcName, ItemData[] stock)
    {
        if(!gameObject.activeSelf)
        {
            this.stock.Clear();
            return;
        }

        this.npcName.text = npcName;

        for (int i = 0; i < stock.Length; i++)
        {
            items[i].UpdateSlot(new InventoryItem(stock[i]));

            this.stock.Add(stock[i]);
        }
        
        currentData = stock[0];

        display.SetUp(
            currentData.itemName,
            currentData.itemDescription,
            currentData.price.ToString()
        );
    }

    protected override void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
            SwitchTo(selectedIndex - 5 < 0 ? selectedIndex + 5 : selectedIndex - 5, true);

        if(Input.GetKeyDown(KeyCode.A))
            SwitchTo(selectedIndex % 5 == 0 ? selectedIndex + 4 : selectedIndex - 1, true);

        if(Input.GetKeyDown(KeyCode.S))
            SwitchTo(selectedIndex + 5 > items.Length - 1 ? selectedIndex - 5 : selectedIndex + 5, true); 

        if(Input.GetKeyDown(KeyCode.D))
            SwitchTo((selectedIndex + 1) % 5 == 0 ? selectedIndex - 4 : selectedIndex + 1, true);

        if(Input.GetKeyDown(KeyCode.C) && currentData != null)
        {
            if(PlayerManager.instance.CanAfford(currentData.price))
            {
                UI.instance.ModifySouls(-currentData.price);
                Inventory.instance.AddItemMute(currentData);
                stock.Remove(currentData);

                // foreach (ItemData item in stock)
                //     Debug.Log(item.itemName);

                currentData = items[selectedIndex + 1]?.item?.itemData;
                items[selectedIndex].CleanUpSlot();

                for (int i = selectedIndex + 1; i < stock.Count + 1; i++)
                {
                    if(items[i].item == null)
                        break;
                    
                    items[i - 1].UpdateSlot(items[i].item);
                    items[i].CleanUpSlot();
                }

                display.SetUp(
                    currentData == null ? "" : currentData.itemName,
                    currentData == null ? "" : currentData.itemDescription,
                    currentData == null ? "" : currentData.price.ToString()
                );
            }
        }
    }
}
