using UnityEngine;

[CreateAssetMenu(fileName = "Heal On Execution", menuName = "Data/Charm Effect/Heal On Execution")]
public class HealOnExecution : ItemEffect
{
    [Range(0f, 1f)]
    [SerializeField] private float percentage;
    private int healAmmount;

    public override void Effect()
    {
        base.Effect();

        healAmmount = Mathf.RoundToInt(stats.health.GetValue() * percentage);

        player.execute.ModifyHealing(true, healAmmount);
    }

    public override void Countereffect()
    {
        player.execute.ModifyHealing(false, 0);
    }
}
