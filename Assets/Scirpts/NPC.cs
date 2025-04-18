using UnityEngine;

public class NPC : MonoBehaviour
{
    public SpriteRenderer sr { get; protected set; }
    public Animator anim { get; protected set; }
    public BoxCollider2D triggerArea { get; protected set; }
    public Dialogue dialogue { get; protected set; }

    protected private void Awake() 
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();    
        triggerArea = GetComponent<BoxCollider2D>();
        dialogue = GetComponent<Dialogue>();
    }

    [SerializeField] protected string npcName;
    protected DialogueManager dialogueManager;
    protected Player player;
    protected bool canStartDialogue;

    private void Start() 
    {
        player = PlayerManager.instance.player;
        dialogueManager = DialogueManager.instance; 
    }

    private void Update() 
    {
        if(canStartDialogue)
        {
            if(Input.GetKeyDown(KeyCode.C))
            {
                player.DialogueStarted();
                dialogueManager.InitializeDialogue(dialogue);
            }
        }    
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.GetComponent<Player>())
        {
            canStartDialogue = true;
            dialogueManager.EnableTalkUI(true);
        }
        
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.GetComponent<Player>())
        {
            canStartDialogue = false;
            dialogueManager.EnableTalkUI(false);
        }
        
    }

    private void OnValidate() 
    {
        gameObject.name = npcName;    
    }

}
