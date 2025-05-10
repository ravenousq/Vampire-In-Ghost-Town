using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    private void Awake() 
    {
        if(!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    [SerializeField] private GameObject dialogueUI;
    [SerializeField] private GameObject talkUI;
    [SerializeField] private GameObject choiceUI;
    [SerializeField] private Scrollbar scrollbar;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private List<Dialogue> dialogues;
    [SerializeField] private float slidingSpeed;
    public Dialogue currentDialogue { get; private set; } = null;
    private bool dialogueOngoing;
    private bool canScroll;
    private float scrollbarCap;
    private bool effect;
    private bool nextLineBlockade;

    private void Start() 
    {
        dialogues = new List<Dialogue>();
        dialogueUI.gameObject.SetActive(false);
        choiceUI.gameObject.SetActive(false);
    }

    private void Update() 
    {
        if(currentDialogue && !dialogueOngoing)
            DisplayCurrentLine();

        if(canScroll)
            ScrollLogic();
        
        if(dialogueOngoing && currentDialogue && Input.GetKeyDown(KeyCode.C) && !effect)
            SkipDialogueLine();
    }

    private void ScrollLogic()
    {
        scrollbar.value = Mathf.MoveTowards(scrollbar.value, scrollbarCap, slidingSpeed * Time.unscaledDeltaTime);

        if (scrollbar.value == scrollbarCap)
        {
            Invoke(nameof(InvokeNextLine), 5f);
            canScroll = false;
        }
    }

    private void DisplayCurrentLine()
    {
        dialogueOngoing = true;
        dialogueText.text = currentDialogue.currentLine.dialogueText;

        Invoke(nameof(StartScrolling), 3f);
    }

    private void StartScrolling()
    {
        canScroll = true;
        CalculateCap();
    }

    public void EnableTalkUI(bool enable)
    {
        if(talkUI != null)
            talkUI.SetActive(enable);
    }

    public void EnableDialogueUI(bool enable)
    {
        if(dialogueUI != null)
            dialogueUI.SetActive(enable);
    }
    
    public void EnableChoiceUI(bool enable)
    {
        if(choiceUI != null)
            choiceUI.SetActive(enable);
    }

    public void SetUpChoices(Dictionary<string, int> choices, ItemData requiredItem = null) => choiceUI.GetComponent<DialogueOptionsUI>().SetUpChoices(choices, requiredItem);

    public void InvokeNextLine() => NextLine();

    public void NextLine(int index = -1)
    {
        if(!dialogueOngoing && effect)
        {
            Debug.Log(dialogueOngoing);
            Debug.Log(effect);
            return;
        }

        if(currentDialogue.currentLine.effect.Length != 0 && !effect)
        {
            effect = true;

            foreach (var effect in currentDialogue.currentLine.effect)
                effect.Effect();
            
            return;
        }

        if(!dialogueUI.gameObject.activeSelf)
            dialogueUI.gameObject.SetActive(true);

        scrollbar.value = 1;
        currentDialogue.NextLine(index);
        dialogueOngoing = false;
        effect = false;
    }

    public void CalculateCap()
    {
        scrollbarCap = 1f;

        for (int i = 0; i < GetLineCount(dialogueText) - 3; i++)
            scrollbarCap -= .18f;
    }

    public static int GetLineCount(TextMeshProUGUI text)
    {
        text.ForceMeshUpdate();
        return text.textInfo.lineCount;
    }

    public void InitializeDialogue(Dialogue dialogue)
    {
        if(!dialogues.Contains(dialogue))
            dialogues.Add(dialogue);

        dialogueUI.gameObject.SetActive(true);

        dialogue.StartDialogue();
        currentDialogue = dialogue;

        EnableTalkUI(false);
    }

    public void SkipDialogueLine()
    {
        CancelInvoke();
        canScroll = false;
        NextLine();
        
    }

    public void EndDialogue()
    {
        currentDialogue.GetComponent<NPC>().DialogueEnded();
        currentDialogue = null;
        dialogueUI.gameObject.SetActive(false);
        PlayerManager.instance.player.DialogueEnded();
    }
}
