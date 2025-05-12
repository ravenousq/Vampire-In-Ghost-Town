using System.Diagnostics;
using UnityEngine;

public class ItemsUI : MonoBehaviour
{
    [SerializeField] protected ItemDisplay display;
    [SerializeField] protected ItemSlotUI[] items;
    protected int selectedIndex = 0;
    protected ItemData currentData = null;

    protected virtual void Awake() 
    {
        items = GetComponentsInChildren<ItemSlotUI>();
    }

    protected virtual void Start() 
    {
        items[selectedIndex].Select(true);
        currentData = items[selectedIndex]?.item?.itemData;
        display.SetUp(currentData);
    }

    protected virtual void Update() 
    {

        if(Input.GetKeyDown(KeyCode.W))
            SwitchTo(selectedIndex - 7 < 0 ? selectedIndex + 21 : selectedIndex - 7);

        if(Input.GetKeyDown(KeyCode.A))
            SwitchTo(selectedIndex % 7 == 0 ? selectedIndex + 6 : selectedIndex - 1);

        if(Input.GetKeyDown(KeyCode.S))
        SwitchTo(selectedIndex + 7 > items.Length - 1 ? selectedIndex - 21 : selectedIndex + 7); 

        if(Input.GetKeyDown(KeyCode.D))
            SwitchTo((selectedIndex + 1) % 7 == 0 ? selectedIndex - 6 : selectedIndex + 1);
    }

    public void SwitchTo(int index = 0, bool price = false)
    {
        if (index != selectedIndex)
            items[selectedIndex].Select(false);

        selectedIndex = index;

        items[selectedIndex].Select(true);

        currentData = items[selectedIndex]?.item?.itemData;
        
        if(!price)
            display.SetUp(currentData);
        else
            display.SetUp(
            currentData?.itemName,
            currentData?.itemDescription,
            currentData?.price.ToString()
        );
    }

}
