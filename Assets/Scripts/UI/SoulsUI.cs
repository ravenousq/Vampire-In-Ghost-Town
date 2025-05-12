using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SoulsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI soulsText;
    [SerializeField] private FadingText textPrefab;
    [SerializeField] private float textSpeed;
    [SerializeField] private float textIdleSpeed;
    private List<FadingText> fadingTexts;
    public int souls { get; private set; }
    private int currentSouls;
    private bool canModifyUI;
    private float minDelay = 0.01f;
    private float maxDelay = 0.2f;
    private FadingText fadingText = null;

    private void Start() 
    {
        fadingTexts = new List<FadingText>();
        souls = PlayerManager.instance.currency;
        currentSouls = souls;
        soulsText.text = currentSouls.ToString();
    }

    private void Update() 
    {
        if (currentSouls != souls && !canModifyUI && fadingText == null)
            StartCoroutine(AddRoutine());
        
    }

    public void ModifySouls(int souls, bool wait = true) 
    {
        fadingTexts.RemoveAll(text => text == null);
        this.souls += souls;

        if(!wait)
        {
            currentSouls = souls;
            return;
        }

        FadingText newText = Instantiate(textPrefab, transform.position - new Vector3(0, 50 + (fadingTexts.Count * 50), 0), Quaternion.identity);
        newText.transform.SetParent(gameObject.transform);
        fadingTexts.Add(newText);
        fadingText = newText;
        fadingText.SetUp((souls < 0 ? "" : "+") + souls.ToString(), textIdleSpeed, textSpeed, transform, soulsText.alignment);
    }

    private IEnumerator AddRoutine()
    {
        canModifyUI = true;

        int step = Mathf.CeilToInt(Mathf.Abs(souls - currentSouls) * 0.05f);
        step = Mathf.Max(1, step);

        float progress = 1f - (float)Mathf.Abs(currentSouls - souls) / Mathf.Abs(souls - currentSouls);
        float delay = Mathf.Lerp(minDelay, maxDelay, Mathf.Pow(progress, 2)); 

        if (currentSouls < souls)
            currentSouls += step;
        else
            currentSouls -= step;

        currentSouls = Mathf.Clamp(currentSouls, Mathf.Min(souls, currentSouls), Mathf.Max(souls, currentSouls));
        soulsText.text = currentSouls.ToString();

        yield return new WaitForSecondsRealtime(delay);

        canModifyUI = false;
    }
    
    public void UpdateSouls()
    {
        souls = PlayerManager.instance.currency;
        currentSouls = souls;
        soulsText.text = souls.ToString();
    }
}
