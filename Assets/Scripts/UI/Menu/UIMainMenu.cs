using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : UIPanel
{
    [SerializeField] private UIBlackout ui_blackout;

    void Start()
    {
        ui_blackout.Initialize();
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
