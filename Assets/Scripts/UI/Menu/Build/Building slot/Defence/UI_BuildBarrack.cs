using UnityEngine;
using TMPro;

public class UI_BuildBarrack : UI_BuildingSlot
{
    [SerializeField] protected TextMeshProUGUI unit;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        D_Barrack barrak = building.GetComponent<D_Barrack>();
        
        _name.text = "Barrack";
        health.text = barrak.GetMaxHP().ToString();
        unit.text = "Warrior";

        foreach(S_CostUI cost in cost)
        {
            cost.amount.text = barrak.GetCostByResource(cost.resourse).ToString();
        }
    }
}