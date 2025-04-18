using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public List<DialogueLine> dialogueTree;
    private DialogueLine firstLine;
    public DialogueLine currentLine { get; private set; }

    private void Start() 
    {

    }

    public void AssignFirstLine(DialogueLine firstLine) => this.firstLine = firstLine;

    public void StartDialogue()
    {
        firstLine = dialogueTree[0];
        currentLine = firstLine;
    }

    public void NextLine() 
    {
        if(currentLine.nextIndex == -1)
        {
            EndDialogue();
            return;
        }

        currentLine = dialogueTree[currentLine.nextIndex];
    }

    void EndDialogue()
    {
        DialogueManager.instance.EndDialogue();
    }

}
