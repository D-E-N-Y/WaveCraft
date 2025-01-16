using UnityEngine;
using TMPro;

public class UI_BuildResidential : UI_BuildingSlot
{
    [SerializeField] protected TextMeshProUGUI villageAmount;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        B_Residential residential = building.GetComponent<B_Residential>();
        
        _name.text = "Residential";
        health.text = residential.GetMaxHP().ToString();
        villageAmount.text = residential.GetVillageAmount().ToString();

        foreach(S_CostUI cost in cost)
        {
            cost.amount.text = residential.GetCostByResource(cost.resourse).ToString();
        }
    }
}