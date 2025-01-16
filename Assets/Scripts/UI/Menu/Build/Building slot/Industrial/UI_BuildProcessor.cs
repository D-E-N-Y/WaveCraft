using UnityEngine;
using TMPro;

public class UI_BuildProcessor : UI_BuildingSlot
{
    [SerializeField] protected TextMeshProUGUI resource;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        I_Processor processor = building.GetComponent<I_Processor>();
        
        _name.text = "Processor";
        health.text = processor.GetMaxHP().ToString();
        resource.text = processor.GetTypeResource().ToString();

        foreach(S_CostUI cost in cost)
        {
            cost.amount.text = processor.GetCostByResource(cost.resourse).ToString();
        }
    }
}