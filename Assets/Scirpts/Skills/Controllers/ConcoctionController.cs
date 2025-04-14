using UnityEngine;

public class ConcoctionController : SkillController
{
    [SerializeField] private int maxConcoctionStacks = 3;
    public int currentConcoctionStacks;
    [Range(0f, 1f)]
    [SerializeField] private float percentage;

    protected override void Start()
    {
        base.Start();
        
        currentConcoctionStacks = maxConcoctionStacks;
    }

    public override bool CanUseSkill()
    {
        if(base.CanUseSkill())
        {
            if(currentConcoctionStacks > 0)
            {
                currentConcoctionStacks--;
                return true;
            }
        }

        return false;
    }

    public int GetHeal() => Mathf.RoundToInt(player.stats.health.GetValue() * percentage);
    public void ModifyPercentage(float percentage) => this.percentage += percentage;
}
