using UnityEngine;

[CreateAssetMenu(fileName = "Increase Currency Effect", menuName = "Data/Charm Effect/Increase Currency Effect")]
public class IncreaseCurrency : ItemEffect
{
    [Range(0f, 1f)]
    [SerializeField] private float percentage;

    public override void Effect()
    {
        base.Effect();

        PlayerManager.instance.AddMultiplier(percentage);
    }

    public override void Countereffect() => PlayerManager.instance.AddMultiplier(-percentage);
}
