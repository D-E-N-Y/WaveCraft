using UnityEngine;

public class LoadingCanvas : MonoBehaviour
{
    [SerializeField] UILoadingScreen ui_loadingScreen;
    [SerializeField] UIBlackout ui_blackout;

    public void Initialize()
    {
        ui_blackout.Initialize();
        ui_blackout.Down();

        ui_loadingScreen.Initialize();
        ui_loadingScreen.Show();

        ui_blackout.finalDown += ui_loadingScreen.StartLoading;
        ui_blackout.finalDown += ui_blackout.ClearFinalDownActions;

        ui_loadingScreen.finalLoading += ui_blackout.Up;
        ui_blackout.finalUp += ui_loadingScreen.Hide;
        ui_blackout.finalUp += ui_blackout.Down;
        ui_blackout.finalUp += ui_blackout.ClearFinalUpActions;
    }

    public UIBlackout GetBlackout() => ui_blackout;
}