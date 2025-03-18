using UnityEngine;

public class Ladder : MonoBehaviour
{
    BoxCollider2D hitbox;

    public int sideToExit;

    void Awake()
    {
        hitbox = GetComponent<BoxCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        other.GetComponent<Player>()?.TriggerLadder(hitbox);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        collision.GetComponent<Player>()?.TriggerLadder(null);
    }
}
