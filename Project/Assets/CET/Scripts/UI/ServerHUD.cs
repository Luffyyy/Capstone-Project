using UnityEngine;

public class ServerHUD : HUDBase
{
    public LevelFinished levelFinished;
    public static ServerHUD Instance;
    private void Awake()
    {
        Instance = this;
    }
    public void PlayFinishedLevelHud()
    {
        levelFinished.Show();
    }
}
