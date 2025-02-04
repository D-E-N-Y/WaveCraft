using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class UI_BuildingSlot : MonoBehaviour
{
    [SerializeField] private GameObject menu_ui;

    [SerializeField] protected GameObject building;
    [SerializeField] protected TextMeshProUGUI _name;
    [SerializeField] protected TextMeshProUGUI health;
    [SerializeField] protected List<S_CostUI> cost;

    public void BuyBuilding()
    {
        Building building = this.building.GetComponent<Building>();
        
        foreach(S_CostUI current in cost)
        {
            if(StorageSystem.current.CheckCountResurces(current.resourse) <= building.GetCostByResource(current.resourse))
            {
                Debug.Log($"not enought {current.resourse}");
                return;
            }
        }

        foreach(S_CostUI current in cost)
        {
            ResourceSystem.current.RemoveResources(current.resourse, building.GetCostByResource(current.resourse));
        }
        
        BuildSystem.current.InitializeWithObject(this.building);
        menu_ui.SetActive(false);
    }
}
