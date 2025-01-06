using UnityEngine;

public class B_Industrial : Building
{
    [SerializeField] private E_Resource resourse;
    
    private void Start() 
    {
        buildingType = E_BuildingType.Industrial;
    }
}
