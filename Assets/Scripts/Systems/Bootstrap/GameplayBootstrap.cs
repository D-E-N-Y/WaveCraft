using System.Collections.Generic;
using UnityEngine;

public class GameplayBootstrap : MonoBehaviour
{
    [SerializeField] private List<GameSystem> systems;
    [SerializeField] private TerrainResourceSpawner terrain;

    [SerializeField] private LoadingCanvas loadingCanvas;
    [SerializeField] private UICanvas uiCanvas;
    [SerializeField] private TooltipCanvas tooltipCanvas;

    [SerializeField] private B_TownHall townHall;

    void Start()
    {
        loadingCanvas.Initialize();

        systems.ForEach(x => x.Initialize());
        terrain.Initialize();

        uiCanvas.Initialize(loadingCanvas.GetBlackout());
        // tooltipCanvas.Initialize();

        townHall.Initialize();
    }
}