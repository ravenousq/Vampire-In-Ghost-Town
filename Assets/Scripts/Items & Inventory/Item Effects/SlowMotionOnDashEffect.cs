using UnityEngine;


[CreateAssetMenu(fileName = "Slow Motion On Dash Effect", menuName = "Data/Charm Effect/Slow Motion On Dash Effect")]
public class SlowMotionOnDashEffect : ItemEffect
{
    public override void Effect()
    {
        base.Effect();

        player.canSlowTime = true;
    }

    public override void Countereffect() => player.canSlowTime = false;
    
}
