using UnityEngine;
using System.Collections.Generic;
using Libs;

[CreateAssetMenu(fileName = "Open Shop", menuName = "Data/Dialogue Effect/Open Shop Effect")]
public class OpenShopEffect : DialogueEffect
{
    [SerializeField] private NPC npc;
    [SerializeField] private SerializableDictionary<string, int> possibleChoices;
    public Dictionary<string, int> choices;
    
    public override void Effect()
    {
        base.Effect();

        choices = possibleChoices.ToDictionary();
        
        manager.EnableDialogueUI(false);
        manager.EnableChoiceUI(true);
        manager.SetUpNPC(DialogueManager.instance.currentDialogue.GetComponent<NPC>());
        manager.SetUpChoices(choices, true);
    }
}
