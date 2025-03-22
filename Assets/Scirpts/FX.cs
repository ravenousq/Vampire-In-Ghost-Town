using System.Collections;
using UnityEngine;

public class FX : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("Flashing")]
    [SerializeField] private float flashDuration;
    [SerializeField] private Material hitMaterial;
    private Material originalMaterial;

    private void Start() 
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMaterial = sr.material;    
    }

    public void Flashing() => StartCoroutine(FlashingRoutine());

    private IEnumerator FlashingRoutine()
    {
        sr.material = hitMaterial;
        Color currentColor = sr.color;

        sr.color = Color.white;
        yield return new WaitForSeconds(flashDuration);

        sr.color = currentColor;
        sr.material = originalMaterial;
    }
}
