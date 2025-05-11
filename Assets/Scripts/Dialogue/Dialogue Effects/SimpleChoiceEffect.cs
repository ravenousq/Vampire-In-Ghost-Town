using UnityEngine;
using System.Collections.Generic;
using Libs;

[CreateAssetMenu(fileName = "Simple Choice Effect", menuName = "Data/Dialogue Effect/Simple Choice Effect")]
public class SimpleChoiceEffect : DialogueEffect
{
    [SerializeField] private SerializableDictionary<string, int> possibleChoices;
    public Dictionary<string, int> choices;

    public override void Effect()
    {
        base.Effect();

        choices = possibleChoices.ToDictionary();
        
        manager.EnableDialogueUI(false);
        manager.EnableChoiceUI(true);
        manager.SetUpChoices(choices);
        
    }
}
