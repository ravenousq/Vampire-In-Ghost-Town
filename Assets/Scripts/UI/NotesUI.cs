using System.Collections;
using System.Security;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NotesUI : ItemsUI
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float defaultFade;
    [SerializeField] private GameObject noteContentDisplay;
    [SerializeField] private Scrollbar scroll;
    private bool focused;
    protected override void Start()
    {
        base.Start();

        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0);
        noteContentDisplay.SetActive(false);
    }

    protected override void Update()
    {
        if (!focused)
            base.Update();

        if (currentData != null && Input.GetKeyDown(KeyCode.C))
        {
            focused = !focused;
            noteContentDisplay.SetActive(focused);
            StartCoroutine(ResetScrollBar());
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, focused ? defaultFade : 0);
        }

        if (Input.GetKey(KeyCode.W))
            scroll.value += .75f * Time.unscaledDeltaTime;
        else if (Input.GetKey(KeyCode.S))
            scroll.value -= .75f * Time.unscaledDeltaTime;
    }

    private IEnumerator ResetScrollBar()
    {
        yield return null; 
        scroll.value = 1;
    }
    
    public void Reset()
    {
        if (focused)
        {
            focused = !focused;
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, focused ? defaultFade : 0);
            noteContentDisplay.SetActive(focused);
        }
    }
}
