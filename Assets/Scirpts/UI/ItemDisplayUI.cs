using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class ItemDisplayUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI myText;
    [SerializeField] private Image[] images;
    [SerializeField] private float appearingSpeed;
    [SerializeField] private float idleTime;
    [SerializeField] private float disappearingDuration;
    private float timer;
    private bool isAppearing = true;
    private float alpha;



    public void SetUp(ItemData item)
    {
        if(item)
        {
            icon.sprite = item.icon;
            myText.text = item.name.ToString();
        }

        images = GetComponentsInChildren<Image>();

        foreach (Image image in images)
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0);

        myText.color = new Color(myText.color.r, myText.color.g, myText.color.b, 0);
    }

    private void Update() 
    {
        timer -= Time.deltaTime;

        if(isAppearing)
            Appear();
        else if(!isAppearing && timer < 0)
            Disappear();
    }

    private void Appear()
    {
        alpha = alpha > .99f ? 1 : Mathf.Lerp(alpha, 1, Time.deltaTime * appearingSpeed);

        foreach (Image image in images)
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);

        myText.color = new Color(myText.color.r, myText.color.g, myText.color.b, alpha);

        if (alpha == 1)
        {
            timer = idleTime;
            isAppearing = false;
        }
    }

    private void Disappear()
    {
        float t = Mathf.Clamp01(-timer / disappearingDuration);

        alpha = alpha < .1f ? 0 : 1f - Mathf.SmoothStep(0f, 1f, t);

        foreach (Image image in images)
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);

        myText.color = new Color(myText.color.r, myText.color.g, myText.color.b, alpha);

        if (alpha == 0)
            Destroy(transform.parent.gameObject);
    }
}
