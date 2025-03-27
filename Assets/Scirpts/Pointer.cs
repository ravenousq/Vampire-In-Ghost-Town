
using UnityEngine;

public class Pointer : MonoBehaviour
{
    private SpriteRenderer sr;
    private void Awake() 
    {
        sr = GetComponent<SpriteRenderer>();    
    }

    private ReapersHalo followedHalo;
    private Vector2 initialPosition;
    private Vector3 bounds;
    private Player player;
    private Camera cam;



    public void SetUp(ReapersHalo halo) => followedHalo = halo;

    private void Start() 
    {
        player = PlayerManager.instance.player;
        cam = Camera.main;
    }

    private void Update()
    {
        UpdateBounds();

        transform.right = transform.position - followedHalo.transform.position;
        
        FollowInBounds();
    }

    private void FollowInBounds()
    {
        Vector3 targetPos = Vector3.MoveTowards(transform.position, followedHalo.transform.position, 100 * Time.deltaTime);

        Vector3 finalDesitnation = new Vector3(Mathf.Clamp(targetPos.x, cam.transform.position.x - bounds.x, cam.transform.position.x + bounds.x),
                                               Mathf.Clamp(targetPos.y, cam.transform.position.y - bounds.y, cam.transform.position.y + bounds.y));

        transform.position = new Vector3(finalDesitnation.x, finalDesitnation.y);
    }

    private void UpdateBounds()
    {
        Vector3 screenCenter = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Vector3 screenTopRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        bounds = new Vector3(screenTopRight.x - screenCenter.x - 1, screenTopRight.y - screenCenter.y - 1);
    }

    public void SwitchSpriteVisibility()
    {
        if(!sr)
            return;

        if(sr.color == Color.white)
            sr.color = Color.clear;
        else
            sr.color = Color.white;
    }
}
