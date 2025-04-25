using System;
using UnityEngine;

[Serializable]
public class DialogueLine
{
    [TextArea(1, 10)] 
    public string dialogueText;
    public int nextIndex;
    public int newFirstLine;
    public AudioClip clip;
    public DialogueEffect[] effect;
    public ItemData[] itemsToGive;
}
