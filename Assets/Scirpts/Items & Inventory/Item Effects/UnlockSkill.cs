using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Unlock Skill", menuName = "Data/Item Effect/Unlock Skill")]
public class UnlockSkill : ItemEffect
{
    [SerializeField] private string skillToUnlock;

    public override void Effect()
    {
        SkillManager.instance.UnlockSkill(skillToUnlock);
    }
}
