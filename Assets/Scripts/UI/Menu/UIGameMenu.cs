using UnityEngine;

public class UIGameMenu : UIPanel 
{
    private UIBlackout ui_blackout;

    private UISystem uiSystem;
    private SceneLoaderSystem sceneLoaderSystem;

    public void Initialize(UIBlackout ui_blackout)
    {
        this.ui_blackout = ui_blackout;

        uiSystem = UISystem.current;
        sceneLoaderSystem = SceneLoaderSystem.current;
    }

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
        uiSystem.CloseCurrentPanel();
    }

    public void Save()
    {

    }

    public void Settings()
    {

    }

    public void ExitToMainMenu()
    {
        isCanClose = false;

        ui_blackout.finalUp += sceneLoaderSystem.LoadMainMenu;
        ui_blackout.Up();
    }

    public void QuitGame()
    {
        isCanClose = false;

        ui_blackout.finalUp += Application.Quit;
        ui_blackout.Up();
    }
}