using UnityEngine;

public enum SkillType
{
    Concoction,
    Dash,
    Halo,
    Parry,
    Shoot,
    Wanted
}
[CreateAssetMenu(fileName = "Modify Skill Cooldown", menuName = "Data/Charm Effect/Modify Skill Cooldown")]
public class ModifySkillCooldown : ItemEffect
{
    [SerializeField] private SkillType skillToModify;
    [Range(0f,1f)]
    [SerializeField] private float cooldownReduction;
    private SkillController skill;
    private float seconds;

    public override void Effect()
    {
        base.Effect();

        skill = SkillToModify();
        seconds = skill.GetCooldown() * cooldownReduction;

        skill.ModifyCooldown(-seconds);
    }

    public override void Countereffect() => skill.ModifyCooldown(seconds);

    public SkillController SkillToModify()
    {
        switch(skillToModify)
        {
            case SkillType.Concoction: 
                return skills.concoction;
            case SkillType.Dash:
                return skills.dash;
            case SkillType.Halo:
                return skills.halo;
            case SkillType.Parry:
                return skills.parry;
            case SkillType.Shoot:
                return skills.shoot;
            case SkillType.Wanted:
                return skills.wanted;
            default:
                return null;
        }
    }
}
