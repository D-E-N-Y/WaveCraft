using System.Collections.Generic;
using UnityEngine;

public class GameplayBootstrap : MonoBehaviour
{
    [SerializeField] private List<GameSystem> systems;
    [SerializeField] private GameObject ui_canvas;
    [SerializeField] private B_TownHall townHall; 

    void Start()
    {
        foreach(GameSystem system in systems)
        {
            system.Initialize();
        }

        ui_canvas.SetActive(true);

        townHall.Initialize();
    }
}