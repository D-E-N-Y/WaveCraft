using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : UIPanel
{
    private UIBlackout ui_blackout;

    public void Initialize(UIBlackout ui_blackout)
    {
        this.ui_blackout = ui_blackout;
    }

    public void NewGameButton()
    {
        ui_blackout.Up();
        ui_blackout.finalUp += SceneLoaderSystem.current.LoadGame;
    }

    public void LoadGameButton()
    {

    }

    public void SettingsButton()
    {

    }

    public void QuitGameButton()
    {
        ui_blackout.Up();
        ui_blackout.finalUp += Application.Quit;
    }
}
