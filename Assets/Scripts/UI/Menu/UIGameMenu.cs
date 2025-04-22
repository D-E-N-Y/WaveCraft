using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIGameMenu : UIPanel 
{
    [SerializeField] private UIBlackBaground ui_blackBackground;
    
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
        SceneManager.LoadScene("MainMenuScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}