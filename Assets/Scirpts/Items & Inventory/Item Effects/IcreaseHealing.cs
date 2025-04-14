using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Increase Healing", menuName = "Data/Charm Effect/Increase Healing")]
public class IcreaseHealing : ItemEffect
{
    [Range(0f,1f)]
    [SerializeField] private float healingIncrease;

    public override void Effect()
    {
        base.Effect();
        player.skills.concoction.ModifyPercentage(healingIncrease);
    } 
    
    public override void Countereffect() => skills.concoction.ModifyPercentage(-healingIncrease);
}
