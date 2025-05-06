using UnityEngine;


[CreateAssetMenu(fileName = "New Key Item", menuName = "Data/Key Item")]
public class KeyItemData : ItemData
{
    public ItemEffect[] pickUpEffects;

    public void PickUpEffect()
    {
        foreach (var effect in pickUpEffects)
        {
            effect.Effect();
        }
    }
}
