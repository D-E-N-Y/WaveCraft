using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class UI_BuildingSlot : MonoBehaviour
{
    [SerializeField] protected Building building;

    [SerializeField] protected TextMeshProUGUI ui_name;
    [SerializeField] protected List<S_CostUI> cost;


    void Start()
    {
        ui_name.text = building.nameActor;

        if (building is D_Pillar)
        {
            ui_name.text = "Wall";
        }

        foreach (S_CostUI current in cost)
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
        
        BuildSystem.current.InitializeWithObject(building);
        UISystem.current.CloseAllPanels();
    }
}
