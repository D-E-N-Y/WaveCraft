using System;
using TMPro;
using UnityEngine;

public class UI_TooltipSpawner : UI_TooltipBuilding
{
    [SerializeField] private TextMeshProUGUI ui_spawnUnit;
    [SerializeField] private TextMeshProUGUI ui_timeToSpawnUnit;
    [SerializeField] private TextMeshProUGUI ui_spawnCost;

    public override Type PanelType => typeof(D_Spawner);

    public override void SetContent(Actor actor, string content, string header = "")
    {
        base.SetContent(actor, content, header);

        D_Spawner spawner = (D_Spawner)actor;

        // ui_spawnUnit.text = spawner.GetTypeUnit().ToString();
        ui_timeToSpawnUnit.text = spawner.GetTimeToBuild().ToString();
        ui_spawnCost.text = spawner.GetCostSpawnUnit().ToString();
    }
}
