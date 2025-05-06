using UnityEngine;


public class DialogueEffect : ScriptableObject
{
    protected Player player;
    protected Inventory inventory;
    protected PlayerStats stats;
    protected SkillManager skills;
    protected DialogueManager manager;
    protected Dialogue dialogue;

    public virtual void Effect()
    {
        player = PlayerManager.instance.player;
        inventory = Inventory.instance;
        stats = player.stats;
        skills = SkillManager.instance;
        manager = DialogueManager.instance;
        dialogue = manager.currentDialogue;
    }
}
