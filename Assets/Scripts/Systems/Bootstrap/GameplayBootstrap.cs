using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayBootstrap : MonoBehaviour
{
    [SerializeField] private List<GameSystem> systems;
    [SerializeField] private TerrainResourceSpawner terrain;
    [SerializeField] private GameObject ui_canvas;
    [SerializeField] private B_TownHall townHall;

    void Start()
    {
        systems.ForEach(x => x.Initialize());
        terrain.Initialize();

        ui_canvas.SetActive(true);
        townHall.Initialize();
    }
}