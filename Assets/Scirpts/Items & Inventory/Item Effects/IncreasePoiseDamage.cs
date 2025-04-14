using UnityEngine;

[CreateAssetMenu(fileName = "IncreasePoiseDamage", menuName = "Data/Charm Effect/Increase Poise Damage")]
public class IncreasePoiseDamage : ItemEffect
{
    [SerializeField] private int points;

    public override void Effect()
    {
        base.Effect();

        skills.shoot.IncreasePoiseDamage(points);
    }

    public override void Countereffect() => skills.shoot.IncreasePoiseDamage(points);
}
