using UnityEngine;

[CreateAssetMenu(fileName = "New Charm", menuName = "Data/Charm")]
public class CharmData : ItemData
{
    public ItemEffect[] equipEffects;

    public void EquipEffects()
    {
        foreach (var effect in equipEffects)
            effect.Effect();
    }

    public void UnequipEffects()
    {
        foreach (var effect in equipEffects)
            effect.Countereffect();
    }
}
