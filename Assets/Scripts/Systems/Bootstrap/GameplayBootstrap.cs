using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayBootstrap : MonoBehaviour
{
    [SerializeField] private List<GameSystem> systems;
    [SerializeField] private TerrainResourceSpawner terrain;

    [SerializeField] private LoadingCanvas loadingCanvas;

    [SerializeField] private UICanvas uiCanvas;
    // [SerializeField] private TooltipCanvas tooltipCanvas;

    [SerializeField] private B_TownHall townHall;

    private void Start()
    {
        loadingCanvas.Initialize(this);
    }

    public void StartInitialize()
    {
        StartCoroutine(Initializing(loadingCanvas.GetLoadingScreen()));
    }

    private IEnumerator Initializing(UILoadingScreen ui_loadingScreen)
    {
        ui_loadingScreen.SetMaxMainProgress(4);

        ui_loadingScreen.SetInitializeText("Systems");
        yield return InitializingSystems(ui_loadingScreen);
        ui_loadingScreen.AddMainProgress();

        ui_loadingScreen.SetInitializeText("Terrain");
        yield return terrain.Initializing(ui_loadingScreen);
        ui_loadingScreen.AddMainProgress();

        ui_loadingScreen.SetInitializeText("UI");
        yield return uiCanvas.Initializing(loadingCanvas.GetBlackout(), ui_loadingScreen);
        ui_loadingScreen.AddMainProgress();

        ui_loadingScreen.SetInitializeText("Buildings");
        yield return InitializingBuildings(ui_loadingScreen);
        ui_loadingScreen.AddMainProgress();
    }

    private IEnumerator InitializingSystems(UILoadingScreen ui_loadingScreen)
    {
        ui_loadingScreen.SetMaxPartProgress(systems.Count);

        foreach (GameSystem system in systems)
        {
            system.Initialize();
            ui_loadingScreen.AddPartProgress();
            yield return null;
        }
    }

    private IEnumerator InitializingBuildings(UILoadingScreen ui_loadingScreen)
    {
        ui_loadingScreen.SetMaxPartProgress(1);

        townHall.Initialize();
        ui_loadingScreen.AddPartProgress();
        yield return null;
    }
}