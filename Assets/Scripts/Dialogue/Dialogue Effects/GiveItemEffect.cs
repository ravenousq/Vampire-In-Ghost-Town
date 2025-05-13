using UnityEngine;
using System.Collections.Generic;
using Libs;

[CreateAssetMenu(fileName = "Give Item Effect", menuName = "Data/Dialogue Effect/Give Item Effect")]
public class GiveItemEffect : DialogueEffect
{
    [SerializeField] private SerializableDictionary<string, int> possibleChoices;
    [SerializeField] private ItemData requiredItem;
    public Dictionary<string, int> choices;

    public override void Effect()
    {
        base.Effect();

        choices = possibleChoices.ToDictionary();
        
        manager.EnableDialogueUI(false);
        manager.EnableChoiceUI(true);
        manager.SetUpChoices(choices, false, requiredItem);
    }
}
