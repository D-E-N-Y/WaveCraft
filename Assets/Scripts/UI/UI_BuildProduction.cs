using UnityEngine;
using TMPro;

public class UI_BuildProduction : UI_Build
{
    [SerializeField] protected TextMeshProUGUI resource;
    [SerializeField] protected TextMeshProUGUI amount;

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
        // amount.text = production.GetMaxAmount().ToString();
        price.text = production.GetCost()[0].count.ToString() + " " + production.GetCost()[1].count.ToString();
    }
}