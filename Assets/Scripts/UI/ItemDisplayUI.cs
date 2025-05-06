using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using System.Diagnostics;

public class ItemDisplayUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI myText;
    [SerializeField] private Image[] images;
    [SerializeField] private float appearingSpeed;
    [SerializeField] private float idleTime;
    [SerializeField] private float disappearingDuration;
    private float timer;
    public bool isAppearing { get; private set; } = true;
    private float alpha;
    private bool confirmation;



    public void SetUp(ItemData item, bool confirmation = false)
    {
        if(confirmation)
        {
            appearingSpeed *= 4;
            idleTime /= 3;
        }

        if(item)
        {
            icon.sprite = item.icon;
            myText.text =  $"Acquired: <color=yellow>{item.itemName}</color>";
            this.confirmation = confirmation;
        }

        images = GetComponentsInChildren<Image>();

        foreach (Image image in images)
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0);

        myText.color = new Color(myText.color.r, myText.color.g, myText.color.b, 0);
    }

    private void Update() 
    {
        timer -= Time.unscaledDeltaTime;

        if(isAppearing)
            Appear();
        else if(!isAppearing && timer < 0)
        {
            if(!confirmation)
                Disappear();
            else
            {
                if(Input.GetKeyDown(KeyCode.C))
                {
                    confirmation = !confirmation;
                    disappearingDuration = .1f;
                    Destroy(transform.parent.gameObject);
                    
                    DialogueManager.instance.Invoke(nameof(DialogueManager.instance.InvokeNextLine), .1f);
                }
            }
        }
        
        
    }

    private void Appear()
    {
        alpha = alpha > .99f ? 1 : Mathf.Lerp(alpha, 1, Time.unscaledDeltaTime * appearingSpeed);

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
