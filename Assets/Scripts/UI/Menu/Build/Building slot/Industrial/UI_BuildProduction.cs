using UnityEngine;
using TMPro;

public class UI_BuildProduction : UI_BuildingSlot
{
    [SerializeField] protected TextMeshProUGUI resource;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        I_Production production = building.GetComponent<I_Production>();
        
        _name.text = "Production";
        health.text = production.GetMaxHP().ToString();
        resource.text = production.GetTypeResource().ToString();

        foreach(S_CostUI cost in cost)
        {
            cost.amount.text = production.GetCostByResource(cost.resourse).ToString();
        }
    }
}