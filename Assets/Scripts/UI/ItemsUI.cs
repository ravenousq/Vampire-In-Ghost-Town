using System.Diagnostics;
using UnityEngine;

public class ItemsUI : MonoBehaviour
{
    [SerializeField] private KeyItemDisplay display;
    [SerializeField] private ItemSlotUI[] items;
    [SerializeField] private int selectedIndex = 0;
    [SerializeField] private ItemData currentData = null;

    private void Awake() 
    {
        items = GetComponentsInChildren<ItemSlotUI>();
    }

    private void Start() 
    {
        items[selectedIndex].Select(true);
        currentData = items[selectedIndex]?.item?.itemData;
        display.SetUp(currentData);
    }

    private void Update() 
    {
        // if(selectedIndex == 0)
        //     display.SetUp(items[selectedIndex]?.item?.itemData);

        if(Input.GetKeyDown(KeyCode.W))
            SwitchTo(selectedIndex - 7 < 0 ? selectedIndex + 21 : selectedIndex - 7);

        if(Input.GetKeyDown(KeyCode.A))
            SwitchTo(selectedIndex - 1 < 0 ? items.Length - 1 : selectedIndex -1);

        if(Input.GetKeyDown(KeyCode.S))
        SwitchTo(selectedIndex + 7 > items.Length - 1 ? selectedIndex - 21 : selectedIndex + 7); 

        if(Input.GetKeyDown(KeyCode.D))
            SwitchTo(selectedIndex + 1 == items.Length ? 0 : selectedIndex +1);
    }

    public void SwitchTo(int index = 0)
    {
        if (index != selectedIndex)
            items[selectedIndex].Select(false);

        selectedIndex = index;

        items[selectedIndex].Select(true);

        currentData = items[selectedIndex]?.item?.itemData;
        display.SetUp(currentData);
    }

}
