using Unity.VisualScripting;
using UnityEngine;

public class CharmsUI : MonoBehaviour
{
    [SerializeField] private ItemDisplay display;
    [SerializeField] private GameObject charmsParent;
    [SerializeField] private GameObject charmsSlotParent;
    private ItemSlotUI[] charms;
    private ItemSlotUI[] charmSlots;
    private int slotIndex;
    private int charmIndex;
    private ItemData currentData;
    private bool navigatingCharms;

    private void Awake() 
    {
        charms = charmsParent.GetComponentsInChildren<ItemSlotUI>();
        charmSlots = charmsSlotParent.GetComponentsInChildren<ItemSlotUI>(); 
    }

    private void Start() 
    {
        SwitchToSlot(0);
    }

    private void Update() 
    {
        if(navigatingCharms)
        {
            if(Input.GetKeyDown(KeyCode.W))
                SwitchToCharm(charmIndex - 7 < 0 ? charmIndex + 14 : charmIndex - 7);

            if(Input.GetKeyDown(KeyCode.A))
                SwitchToCharm(charmIndex - 1 < 0 ? charms.Length - 1 : charmIndex -1);

            if(Input.GetKeyDown(KeyCode.S))
            SwitchToCharm(charmIndex + 7 > charms.Length - 1 ? charmIndex - 14 : charmIndex + 7); 

            if(Input.GetKeyDown(KeyCode.D))
                SwitchToCharm(charmIndex + 1 == charms.Length ? 0 : charmIndex +1);
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.A))
                SwitchToSlot(slotIndex - 1 < 0 ? charmSlots.Length - 1 : slotIndex - 1);

            if(Input.GetKeyDown(KeyCode.D))
                SwitchToSlot(slotIndex + 1 == charmSlots.Length ? 0 : slotIndex + 1);
            
        }   

        if(Input.GetKeyDown(KeyCode.C)) 
        {
            navigatingCharms = !navigatingCharms;

            if(navigatingCharms)
            {
                SwitchToCharm(charmIndex);
            }
            else
            {
                if(charms[charmIndex]?.item?.itemData != null)
                    Inventory.instance.EquipCharm(charms[charmIndex].item.itemData as CharmData, slotIndex);

                SwitchToCharm(-1);
            }
        }
    }

    public void SwitchToSlot(int index)
    {
        if(index != slotIndex)
            charmSlots[slotIndex].Select(false);
        
        slotIndex = index;

        charmSlots[slotIndex].Select(true);
        currentData = charmSlots[slotIndex]?.item?.itemData;
        display.SetUp(currentData);
    }

    public void SwitchToCharm(int index)
    {
        if(index == -1)
        {
            charms[charmIndex].Select(false);
            return;
        }
        

        if(index != charmIndex)
            charms[charmIndex].Select(false);

        charmIndex = index;

        charms[charmIndex].Select(true);
        currentData = charms[charmIndex]?.item?.itemData;
        display.SetUp(currentData);
    }
}
