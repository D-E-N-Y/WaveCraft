using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class UI_BuildingSlot : MonoBehaviour
{
    [SerializeField] protected GameObject prefab;
    [SerializeField] protected List<S_CostUI> cost;

    private Building building;

    void Start()
    {
        building = prefab.GetComponent<Building>();

        foreach(S_CostUI current in cost) 
        {
            current.amount.text = building.GetCostByResource(current.resourse).ToString();
        }
    }

    public void BuyBuilding()
    {
        foreach(S_CostUI current in cost) 
        {
            if(StorageSystem.current.CheckCountResurces(current.resourse) < building.GetCostByResource(current.resourse))
            {
                Debug.Log($"not enought {current.resourse}");
                return;
            }
        }

        foreach(S_CostUI current in cost)
        {
            ResourceSystem.current.RemoveResources(current.resourse, building.GetCostByResource(current.resourse));
        }
        
        BuildSystem.current.InitializeWithObject(building);
        UISystem.current.CloseAllPanels();
    }
}
