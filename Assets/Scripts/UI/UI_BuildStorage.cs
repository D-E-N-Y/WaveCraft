using UnityEngine;
using TMPro;

public class UI_BuildStorage : UI_Build
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
        
        name.text = "Storage";
        health.text = storage.GetMaxHP().ToString();
        resource.text = storage.GetTypeResource().ToString();
        amount.text = storage.GetMaxAmount().ToString();
        price.text = storage.GetCost()[0].count.ToString() + " " + storage.GetCost()[1].count.ToString();
    }
}