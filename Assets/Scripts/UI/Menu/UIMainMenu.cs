using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    public void NewGameButton()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void LoadGameButton()
    {

    }

    public void SettingsButton()
    {

    }

    public void QuitGameButton()
    {
        Application.Quit();
    }
}
