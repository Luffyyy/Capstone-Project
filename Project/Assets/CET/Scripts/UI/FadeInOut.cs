using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] CanvasGroup canvasGroup;

    [Header("Settings")]
    [SerializeField, Range(0.01f, 10)]
    [Tooltip("Time in seconds to fade in")]
    float fadeInTime = 2;

    [SerializeField, Range(0.01f, 10)]
    [Tooltip("Time in seconds to fade out")]
    float fadeOutTime = 2;

    bool isFading;

    void Awake()
    {
        canvasGroup.alpha = 1;
        StartCoroutine(FadeOut());
    }

    void OnValidate()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponentInChildren<CanvasGroup>();
    }

    public float GetFadeInTime() => fadeInTime + Time.fixedDeltaTime;

    public IEnumerator FadeIn(bool instant=false)
    {
        yield return FadeImage(0f, 1f, instant ? 0 : fadeInTime);
    }

    public float GetFadeOutTime() => fadeOutTime + Time.fixedDeltaTime;

    public IEnumerator FadeOut()
    {
        yield return FadeImage(1f, 0f, fadeOutTime);
    }

    private IEnumerator FadeImage(float startAlpha, float endAlpha, float duration)
    {
        if (canvasGroup == null) yield break;

        if (isFading) yield break;

        // Short circuit if the alpha is already at endAlpha
        if (Mathf.Approximately(canvasGroup.alpha, endAlpha)) yield break;

        isFading = true;

        float elapsedTime = 0f;
        float fixedDeltaTime = Time.fixedDeltaTime;

        while (elapsedTime < duration)
        {
            elapsedTime += fixedDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            yield return new WaitForFixedUpdate();
        }

        // Ensure the final alpha value is set
        canvasGroup.alpha = endAlpha;

        isFading = false;
    }
}
