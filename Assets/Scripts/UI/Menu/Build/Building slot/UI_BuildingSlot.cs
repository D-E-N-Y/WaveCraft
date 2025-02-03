using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class UI_BuildingSlot : MonoBehaviour
{
    [SerializeField] protected GameObject building;
    [SerializeField] protected TextMeshProUGUI _name;
    [SerializeField] protected TextMeshProUGUI health;
    [SerializeField] protected List<S_CostUI> cost;

    public void BuyBuilding()
    {
        BuildSystem.current.InitializeWithObject(building);
    }
}
