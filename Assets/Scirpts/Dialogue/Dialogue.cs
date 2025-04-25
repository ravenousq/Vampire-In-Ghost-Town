using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public List<DialogueLine> dialogueTree;
    private DialogueLine firstLine;
    public DialogueLine currentLine { get; private set; }

    private void Start() 
    {
        firstLine = dialogueTree[0];
    }

    public void AssignFirstLine(DialogueLine firstLine) => this.firstLine = firstLine;

    public void StartDialogue()
    {
        currentLine = firstLine;
    }

    public void NextLine(int index) 
    {
        if(index > -1)
        {
            currentLine = dialogueTree[index];
            return;
        }

        if(currentLine.newFirstLine != 0)
            AssignFirstLine(dialogueTree[currentLine.newFirstLine]);

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
