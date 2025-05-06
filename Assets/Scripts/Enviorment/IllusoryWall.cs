using UnityEngine;

public class IllusoryWall : MonoBehaviour
{
    private SpriteRenderer sr;

    private void Awake() 
    {
        sr = GetComponent<SpriteRenderer>();
    }

    [SerializeField] private int requiredFacingDir;
    [SerializeField] private float fadeSpeed;
    private Player player;
    private bool dispell;
    [SerializeField] private BoxCollider2D wall;

    private void Update() 
    {
        if(player && SkillManager.instance.isSkillUnlocked("Constellation of Tears"))
        {
            if(Input.GetKeyDown(KeyCode.Mouse0) && player.facingDir == requiredFacingDir && Time.timeScale != 0 && player.IsGroundDetected())
                wall.enabled = false;
        }

        if(!wall.enabled)
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, Mathf.MoveTowards(sr.color.a, 0, fadeSpeed * Time.deltaTime));
        
        if(sr.color.a == 0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.GetComponent<Player>())
            player  = other.GetComponent<Player>();
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.GetComponent<Player>())
            player = null;
    }
}
