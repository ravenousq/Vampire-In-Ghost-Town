using NUnit.Framework;
using UnityEngine;

public class AfterImage : MonoBehaviour
{
    private float lifeTime = .5f;
    [SerializeField] private float fadeSpeed;
    private SpriteRenderer sr => GetComponent<SpriteRenderer>();

    public void SetUpSprite(Sprite sprite, bool facingRight, Transform container = null)
    {
        if(!facingRight)
            transform.Rotate(0, 180, 0);
        sr.sprite = sprite;

        if(container)
            transform.parent = container.transform;

        if(GetComponentInParent<ReapersHalo>())
        {
            sr.color = new Color(118, 118, 118, 255);
            transform.parent = null;
        }
    }
    
    
    private void Start() 
    {
        Destroy(gameObject, lifeTime + .1f);
    }

    private void Update() 
    {
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, Mathf.Lerp(sr.color.a, 0, fadeSpeed));
    }

}
