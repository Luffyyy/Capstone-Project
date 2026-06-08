using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScreen : MonoBehaviour
{
    public void ReturnToMainMenu() {
        SceneManager.LoadScene("MainMenuScene");
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
