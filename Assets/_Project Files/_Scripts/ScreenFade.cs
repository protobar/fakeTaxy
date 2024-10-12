using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFade : MonoBehaviour
{
    /*public Image fadeImage; // The UI Image used for fading
    public float fadeDuration = 1f;

    private void Awake()
    {
        if (fadeImage == null)
        {
            fadeImage = GetComponent<Image>();
            if (fadeImage == null)
            {
                Debug.LogError("Fade Image not assigned and not found on the same GameObject.");
            }
        }
    }

    public void FadeOut()
    {
        StartCoroutine(FadeTo(1f)); // Fade to black
    }

    public void FadeIn()
    {
        StartCoroutine(FadeTo(0f)); // Fade to transparent
    }

    private IEnumerator FadeTo(float targetAlpha)
    {
        float startAlpha = fadeImage.color.a;
        float time = 0f;

        Color color = fadeImage.color;

        while (time < fadeDuration)
        {
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            color.a = alpha;
            fadeImage.color = color;
            time += Time.deltaTime;
            yield return null;
        }

        color.a = targetAlpha;
        fadeImage.color = color;
    }*/
}
