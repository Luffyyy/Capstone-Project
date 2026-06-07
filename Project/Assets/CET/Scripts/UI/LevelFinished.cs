using System.Collections;
using UnityEngine;

public class LevelFinished : MonoBehaviour
{
    public RectTransform panel;
    public AudioSource audioSource;
    private void Awake()
    {
        gameObject.SetActive(false);
    }
    public void Show()
    {
        Debug.Log("LevelFinished menu opened");
        gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(ShowAnimation());
    }
    private IEnumerator ShowAnimation()
    {
        Debug.Log("Playing level finished animation");
        audioSource.Play();
        panel.localScale = Vector3.zero;
        float duration = 0.3f;
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            panel.localScale = Vector3.Lerp(Vector3.zero,Vector3.one,t / duration);
            yield return null;
        }
        panel.localScale = Vector3.one;
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
    }
}