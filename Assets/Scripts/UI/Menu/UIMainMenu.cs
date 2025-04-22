using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : UIPanel
{
    [SerializeField] private UILoadingScreen ui_loadingScreen;
    
    public void NewGameButton()
    {
        ui_loadingScreen.Show();
        ui_loadingScreen.Initialize();

        Hide();
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
