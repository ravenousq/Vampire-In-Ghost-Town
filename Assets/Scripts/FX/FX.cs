using System.Collections;
using UnityEngine;

public class FX : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("Flashing")]
    [SerializeField] private float flashDuration;
    [SerializeField] private Material hitMaterial;
    [Tooltip("For Player FX only.")]
    [SerializeField] private GameObject reloadTorso;
    private SpriteRenderer reloadSR;
    private Material originalMaterial;
    private Color originalColor;
    private float iFramesTimer;

    private void Start() 
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        if(reloadTorso)
            reloadSR = reloadTorso.GetComponent<SpriteRenderer>();

        originalMaterial = sr.material;
        originalColor = sr.color;
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

        if(reloadSR && reloadTorso.activeSelf)
        {
            reloadSR.material = hitMaterial;
            reloadSR.color = Color.white;
        }
        yield return new WaitForSeconds(flashingDuration);

        sr.color = currentColor;
        sr.material = originalMaterial;

        if(reloadSR && reloadTorso.activeSelf)
        {
            reloadSR.material = originalMaterial;
            reloadSR.color = currentColor;
        }
    }

    public void IFramesFlashing(float seconds)
    {
        iFramesTimer = seconds;

        StartCoroutine(IFramesFlashingRoutine(.15f));
    }

    public IEnumerator IFramesFlashingRoutine(float seconds)
    {
        StartCoroutine(FlashingRoutine(seconds));

        yield return new WaitForSeconds(seconds * 2);

        if(iFramesTimer > 0)
            StartCoroutine(IFramesFlashingRoutine(seconds));
    }

    public void ResetSprite()
    {
        sr.material = originalMaterial;
        sr.color = originalColor;
    }
}
