using UnityEngine;
using TMPro;

public class UI_BuildStorage : UI_BuildingSlot
{
    [SerializeField] protected TextMeshProUGUI resource;
    [SerializeField] protected TextMeshProUGUI amount;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        I_Storage storage = building.GetComponent<I_Storage>();
        
        _name.text = "Storage";
        health.text = storage.GetMaxHP().ToString();
        resource.text = storage.GetTypeResource().ToString();
        amount.text = storage.GetMaxAmount().ToString();
        
        foreach(S_CostUI cost in cost)
        {
            cost.amount.text = storage.GetCostByResource(cost.resourse).ToString();
        }
    }
}