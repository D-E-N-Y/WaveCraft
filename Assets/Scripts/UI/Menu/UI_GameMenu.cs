using UnityEngine;
using UnityEngine.InputSystem;

public class UI_GameMenu : MonoBehaviour
{
    public void Continue()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    public void Save()
    {

    }

    public void Settings()
    {

    }

    public void ExitToMainMenu()
    {

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
