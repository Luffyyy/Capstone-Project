using Mirror;
using UnityEngine;

public class PlayerCustomizer : NetworkBehaviour
{
    [SyncVar(hook=nameof(OnSetColorIndex))]
    public int ColorIndex;
    [SyncVar(hook=nameof(OnSetEmotionIndex))]
    public int EmotionIndex;

    void OnSetColorIndex(int oldVal, int newVal)
    {
        SetColorIndex(newVal);
    }

    // Update is called once per frame
    void OnSetEmotionIndex(int oldVal, int newVal)
    {
        SetEmotionIndex(newVal);
    }

    public void SetColorIndex(int colorIndex)
    {
        ColorIndex = colorIndex;
        GetComponent<Rob10ColorManager>().ChangeBodyColor(colorIndex);
    }

    public void SetEmotionIndex(int emotionIndex)
    {
        EmotionIndex = emotionIndex;
        GetComponent<EmotionChanger>().SetEmotionEyes(emotionIndex);
        GetComponent<EmotionChanger>().SetEmotionMouth(emotionIndex);
    }
}
