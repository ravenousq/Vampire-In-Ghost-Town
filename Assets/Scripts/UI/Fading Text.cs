
using TMPro;
using UnityEngine;

public class FadingText : MonoBehaviour
{
    private Transform target;
    private float speed;
    private float timer;
    private TextMeshProUGUI myText;
    private float defaultDistance;
    

    public void SetUp(string ammount, float idleTime, float speed, Transform target, TextAlignmentOptions alignment = TextAlignmentOptions.MidlineRight)
    {
        myText = GetComponent<TextMeshProUGUI>();
        myText.text = ammount;
        myText.alignment = alignment;
        timer = idleTime;
        this.speed = speed;
        this.target = target;
        defaultDistance = Vector2.Distance(transform.position, target.position);
    }

    private void Update() 
    {
        timer -= Time.unscaledDeltaTime;

        if(timer < 0)
        {
            myText.color = new Color(myText.color.r, myText.color.g, myText.color.b, Mathf.Clamp01(Vector2.Distance(transform.position, target.position)/ defaultDistance));
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.unscaledDeltaTime);
        }
        
        if(Vector2.Distance(transform.position, target.position) < 1)
            Destroy(gameObject);
    }
}
