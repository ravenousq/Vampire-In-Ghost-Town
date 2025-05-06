using UnityEngine;

[CreateAssetMenu(fileName = "Boost Poise Recovery Effect", menuName = "Data/Charm Effect/Boost Poise Recovery Effect")]
public class BoostPoiseRecovery : ItemEffect
{
    [SerializeField] private int modifier;

    public override void Effect()
    {
        base.Effect();

        stats.ModifyPoiseRecovery(modifier);
    }

    public override void Countereffect() => stats.ModifyPoiseRecovery(-modifier);
}
