using UnityEngine;

public class ItemEffect : ScriptableObject
{
    protected Player player;
    protected Inventory inventory;
    protected PlayerStats stats;
    protected SkillManager skills;

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
        inventory = Inventory.instance;
        stats = player.stats;
        skills = SkillManager.instance;
    }

    public virtual void Effect()
    {
        Debug.Log("Effect Executed");
    }

    public virtual void Countereffect()
    {
       Debug.Log("Countereffect Executed"); 
    }
}
