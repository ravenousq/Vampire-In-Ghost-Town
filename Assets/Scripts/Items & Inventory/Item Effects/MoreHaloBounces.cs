using UnityEngine;

[CreateAssetMenu(fileName = "More Halo Bounces Effect", menuName = "Data/Charm Effect/More Halo Bounces Effect")]
public class MoreHaloBounces : ItemEffect
{
    [SerializeField] private int modifier;

    public override void Effect()
    {
        base.Effect();

        skills.halo.ModifyBounces(modifier);
    }

    public override void Countereffect() => skills.halo.ModifyBounces(-modifier);
}
