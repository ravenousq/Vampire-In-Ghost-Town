using UnityEngine;

public class Entity : MonoBehaviour
{
    #region Components
    protected Rigidbody2D rb;
    protected Collider2D cd;
    protected SpriteRenderer sr;
    #endregion



    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }
}
