using System;
using UnityEngine;

[Serializable]
public class DialogueLine
{
    [TextArea(1, 10)] 
    public string dialogueText;
    public int nextIndex;
    public AudioClip clip;
    public ItemData item;
    public DialogueEffect effect;
}
