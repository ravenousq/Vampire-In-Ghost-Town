using UnityEngine;

[CreateAssetMenu(fileName = "Increase Offensive Stats", menuName = "Data/Item Effect/Increase Offensive Stats")]
public class IncreaseOffensiveStats : ItemEffect
{
    [Range(0f, 1f)]
    [SerializeField] private float damageIncrease;
    [Range(1,20)]
    [SerializeField] private int agilityIncrease;
    [Range(1,20)]
    [SerializeField] private int brutalityIncrease;

    public override void Effect()
    {
        base.Effect();

        stats.damage.AddModifier(Mathf.RoundToInt(stats.damage.GetValue() * damageIncrease));
        stats.agility.AddModifier(agilityIncrease);
        stats.brutality.AddModifier(brutalityIncrease);
    }
}
