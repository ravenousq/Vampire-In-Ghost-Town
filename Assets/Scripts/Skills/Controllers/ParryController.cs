
public class ParryController : SkillController
{
    public int parryPoiseDamage;
    public float parryWindow;

    public override void UseSkill()
    {
        base.UseSkill();
    }

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }
}
