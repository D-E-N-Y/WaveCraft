using UnityEngine;
using TMPro;

public class UI_BuildArchersTower : UI_BuildingSlot
{
    [SerializeField] protected TextMeshProUGUI unit;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        D_ArcherTower archerTower = building.GetComponent<D_ArcherTower>();
        
        _name.text = "Archer's Tower";
        health.text = archerTower.GetMaxHP().ToString();
        unit.text = "Archer";

        foreach(S_CostUI cost in cost)
        {
            cost.amount.text = archerTower.GetCostByResource(cost.resourse).ToString();
        }
    }
}