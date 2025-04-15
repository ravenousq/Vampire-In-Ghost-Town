using UnityEngine;

[CreateAssetMenu(fileName = "Increase Deffensive Stats", menuName = "Data/Item Effect/Increase Deffensive Stats")]
public class IncreaseDeffensiveStats : ItemEffect
{
    [Range(0f, 1f)]
    [SerializeField] private float healthIncrease;
    [Range(1,20)]
    [SerializeField] private int poiseIncrease;
    [Range(1,20)]
    [SerializeField] private int armorIncrease;


    public override void Effect()
    {
        base.Effect();

        stats.health.AddModifier(Mathf.RoundToInt(stats.health.GetValue() * healthIncrease));

        stats.Heal(stats.health.GetValue());
        stats.poise.AddModifier(poiseIncrease);
        stats.armor.AddModifier(armorIncrease);
    }
}
