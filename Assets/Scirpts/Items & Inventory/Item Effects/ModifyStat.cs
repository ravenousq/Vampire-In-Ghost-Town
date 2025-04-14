using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
public enum StatType
{
    health,
    poise,
    armor, 
    damage,
    agility,
    brutality,
}

[CreateAssetMenu(fileName = "Modify Stat", menuName = "Data/Charm Effect/Modify Stat")]

public class ModifyStat : ItemEffect
{
    [SerializeField] private StatType stat;
    [SerializeField] private int modifier;
    private Stat statToModify;

    public override void Effect()
    {
        base.Effect();

        statToModify = StatToModify();

        if(stat == StatType.health)
            statToModify.AddModifier(stats.health.GetValue() / modifier);
        else
            statToModify.AddModifier(modifier);
    }

    public override void Countereffect() => statToModify.RemoveModifier(modifier);
    

    private Stat StatToModify()
    {
        switch (stat)
        {
            case StatType.health: return stats.health;
            case StatType.poise: return stats.poise;
            case StatType.armor: return stats.armor;
            case StatType.damage: return stats.damage;
            case StatType.agility: return stats.agility;
            case StatType.brutality: return stats.brutality;
            default: return null;
        }
    }
}

