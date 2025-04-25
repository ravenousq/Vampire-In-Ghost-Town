using UnityEngine;


[CreateAssetMenu(fileName = "Receive Item Effect", menuName = "Data/Dialogue Effect/Receive Effect")]
public class ReceiveItemEffect : DialogueEffect
{
    private ItemData[] itemsToGive;

    public override void Effect()
    {
        base.Effect();

        itemsToGive = manager.currentDialogue.currentLine.itemsToGive;

        manager.EnableDialogueUI(false);
        foreach(ItemData item in itemsToGive)
            inventory.AddItem(item, true);
    }

}
