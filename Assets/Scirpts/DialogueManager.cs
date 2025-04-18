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
    [SerializeField] private Scrollbar scrollbar;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private List<Dialogue> dialogues;
    [SerializeField] private float slidingSpeed;
    private Dialogue currentDialogue = null;
    private bool dialogueOngoing;
    private bool canScroll;
    private float scrollbarCap;

    private void Start() 
    {
        dialogues = new List<Dialogue>();
        dialogueUI.gameObject.SetActive(false);
    }

    private void Update() 
    {
        if(currentDialogue && !dialogueOngoing)
        {
            dialogueOngoing = true;
            dialogueText.text = currentDialogue.currentLine.dialogueText;

            Invoke(nameof(StartScrolling), 3f);
        }

        if(canScroll)
        {
            scrollbar.value = Mathf.MoveTowards(scrollbar.value, scrollbarCap, slidingSpeed * Time.unscaledDeltaTime);
            
            if(scrollbar.value == scrollbarCap)
            {
                Invoke(nameof(NextLine), 5f);
                canScroll = false;
            }
        }

    }

    private void StartScrolling()
    {
        canScroll = true;
        CalculateCap();
    }

    public void EnableTalkUI(bool enable) => talkUI.gameObject.SetActive(enable);
    
    public void NextLine()
    {
        scrollbar.value = 1;
        currentDialogue.NextLine();
        dialogueOngoing = false;
    }

    public void CalculateCap()
    {
        scrollbarCap = 1f;

        for (int i = 0; i < GetLineCount(dialogueText) - 3; i++)
            scrollbarCap -= .2f;
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

    public static int GetLineCount(TextMeshProUGUI text)
    {
        text.ForceMeshUpdate();
        return text.textInfo.lineCount;
    }

    public void EndDialogue()
    {
        currentDialogue = null;
        dialogueUI.gameObject.SetActive(false);
        PlayerManager.instance.player.dialogue.EndDialogue();
    }
}
