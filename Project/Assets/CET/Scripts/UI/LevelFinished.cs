using System.Collections;
using UnityEngine;

public class LevelFinished : MenuBase
{
    public RectTransform panel;
    public override void Show()
    {
        Debug.Log("LevelFinished menu opened");
        base.Show();
        StopAllCoroutines();
        StartCoroutine(ShowAnimation());
    }
    private IEnumerator ShowAnimation()
    {
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
        MenuManager.Instance.CloseCurrentMenu();
    }
}