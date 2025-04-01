using System.Collections;
using UnityEngine;

public class FX : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("Flashing")]
    [SerializeField] private float flashDuration;
    [SerializeField] private Material hitMaterial;
    private Material originalMaterial;
    private float iFramesTimer;

    private void Start() 
    {
        sr = GetComponentInChildren<SpriteRenderer>();

        originalMaterial = sr.material;    
    }

    private void Update() 
    {
        iFramesTimer -= Time.deltaTime;    
    }

    public void Flashing() => StartCoroutine(FlashingRoutine(flashDuration));

    private IEnumerator FlashingRoutine(float flashingDuration)
    {
        sr.material = hitMaterial;
        Color currentColor = sr.color;

        sr.color = Color.white;
        yield return new WaitForSeconds(flashingDuration);

        sr.color = currentColor;
        sr.material = originalMaterial;
    }

    public void IFramesFlashing(float seconds)
    {
        iFramesTimer = seconds;

        StartCoroutine(IFramesFlashingRoutine(.15f));
    }

    private IEnumerator IFramesFlashingRoutine(float seconds)
    {
        StartCoroutine(FlashingRoutine(seconds));

        yield return new WaitForSeconds(seconds * 2);

        if(iFramesTimer > 0)
            StartCoroutine(IFramesFlashingRoutine(seconds));
    }
}
