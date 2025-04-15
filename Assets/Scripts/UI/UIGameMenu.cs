using UnityEngine;

public class UIGameMenu : UIPanel 
{
    void OnEnable()
    {
        Time.timeScale = 0f;
    }

    void OnDisable()
    {
        Time.timeScale = 1f;
    }

    public void Continue()
    {
        UISystem.current.CloseCurrentPanel();
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