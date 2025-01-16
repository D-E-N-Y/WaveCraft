using UnityEngine;
using TMPro;

public class UI_BuildMagesTower : UI_BuildingSlot
{
    [SerializeField] protected TextMeshProUGUI unit;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        D_MageTower magesTower = building.GetComponent<D_MageTower>();
        
        _name.text = "Mege's Tower";
        health.text = magesTower.GetMaxHP().ToString();
        unit.text = "Mage";

        foreach(S_CostUI cost in cost)
        {
            cost.amount.text = magesTower.GetCostByResource(cost.resourse).ToString();
        }
    }
}