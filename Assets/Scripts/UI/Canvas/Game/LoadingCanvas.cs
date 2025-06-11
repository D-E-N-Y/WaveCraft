using UnityEngine;

public class LoadingCanvas : MonoBehaviour
{
    [SerializeField] UILoadingScreen ui_loadingScreen;
    [SerializeField] UIBlackout ui_blackout;

    public void Initialize(GameplayBootstrap gameplayBootstrap)
    {
        ui_blackout.Initialize();
        ui_blackout.Down();

        ui_loadingScreen.Initialize();
        ui_loadingScreen.Show();

        ui_blackout.finalDown += gameplayBootstrap.StartInitialize;
        ui_blackout.finalDown += ui_blackout.SetUnscaledTimeUpdateMode;
        ui_blackout.finalDown += ui_blackout.ClearFinalDownActions;

        ui_loadingScreen.completeMainLoading += ui_blackout.Up;
        ui_blackout.finalUp += ui_loadingScreen.Hide;
        ui_blackout.finalUp += ui_blackout.Down;
        ui_blackout.finalUp += ui_blackout.ClearFinalUpActions;
    }

    public UILoadingScreen GetLoadingScreen() => ui_loadingScreen;
    public UIBlackout GetBlackout() => ui_blackout;
}