using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] private Image deathScreen;
    [SerializeField] private TextMeshProUGUI deathText;
    [SerializeField] private float fadeOutSpeed;
    private bool isFading;


    private void Update() 
    {
        if(isFading)
        {
            float alpha = Mathf.MoveTowards(deathScreen.color.a, 1, fadeOutSpeed * Time.unscaledDeltaTime);

            deathScreen.color = new Color(deathScreen.color.r, deathScreen.color.g, deathScreen.color.b, alpha);
            deathText.color = new Color(deathText.color.r, deathText.color.g, deathText.color.b, alpha);

            if(alpha == 1)
            {
                isFading = false;
                StartCoroutine(ResetGameRoutine());
            }
        }
    }

    public void ActivateDeathScreen() => isFading = true;

    private IEnumerator ResetGameRoutine()
    {
        yield return new WaitForSecondsRealtime(3);

        ResetGame();
    }

    private void ResetGame() => Application.Quit();
}
