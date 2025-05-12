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
    [SerializeField] protected Transform dialoguePoint;
    [Tooltip("-1 => facing left, 1 => facing right")]
    [SerializeField] protected int requiredFacingDir;

    [Header("Shop")]
    [SerializeField] public ItemData[] stock;
    protected DialogueManager dialogueManager;
    protected Player player;
    protected bool canStartDialogue;
    private float timer = .3f;
    private bool dialogueOngoing;
    private bool inRange;

    private void Start() 
    {
        player = PlayerManager.instance.player;
        dialogueManager = DialogueManager.instance; 
    }

    private void Update() 
    {
        if(inRange && !dialogueOngoing)
            timer -= Time.deltaTime;

        if(timer < 0 && !canStartDialogue)
        {
            dialogueManager.EnableTalkUI(true);
            canStartDialogue = true;
        }

        if(canStartDialogue)
        {
            if(Input.GetKeyDown(KeyCode.C) && player.IsGroundDetected() && Time.timeScale == 1)
            {   
                timer = 10;
                canStartDialogue = false;
                dialogueOngoing = true;
                player.DialogueStarted(dialoguePoint, requiredFacingDir);
                dialogueManager.InitializeDialogue(dialogue);
            }
        }    
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.GetComponent<Player>())
            inRange = true;
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.GetComponent<Player>())
        {
            inRange = false;
            canStartDialogue = false;
            dialogueManager.EnableTalkUI(false);
            timer = .3f;
        }
    }

    public void DialogueEnded()
    {
        timer = 1;
        dialogueOngoing = false;
    } 

    private void OnValidate() 
    {
        gameObject.name = npcName;    
    }

}
