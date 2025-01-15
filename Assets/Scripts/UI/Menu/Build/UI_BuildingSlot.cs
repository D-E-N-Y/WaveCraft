using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class UI_BuildingSlot : MonoBehaviour
{
    [Serializable]
    public struct S_CostUI
    {
        public E_Resource resourse;
        public TextMeshProUGUI amount;

        public S_CostUI (E_Resource resourse, TextMeshProUGUI amount)
        {
            this.resourse = resourse;
            this.amount = amount;
        }
    }
    
    [SerializeField] protected GameObject building;

    [SerializeField] protected TextMeshProUGUI _name;
    [SerializeField] protected TextMeshProUGUI health;
    [SerializeField] protected List<S_CostUI> cost;

    public void BuyBuilding()
    {
        Debug.Log("building");
        
        BuildSystem.current.InitializeWithObject(building);
    }
}
