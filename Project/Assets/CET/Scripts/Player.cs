using Mirror;
using UnityEngine;

public class PlayerSaveData
{
    public int PlayerIndex;
    public int ColorIndex;
    public int EmotionIndex;
}

public class Player : NetworkBehaviour
{
    [SyncVar, HideInInspector]
    public int PlayerIndex;

    [SyncVar(hook=nameof(OnSetColorIndex))]
    public int ColorIndex;
    [SyncVar(hook=nameof(OnSetEmotionIndex))]
    public int EmotionIndex;

    public PlayerSaveData GetData() => new()
    {
        PlayerIndex = PlayerIndex,
        ColorIndex = ColorIndex,
        EmotionIndex =  EmotionIndex
    };

    void OnSetColorIndex(int oldVal, int newVal)
    {
        SetColorIndex(newVal);
    }

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
