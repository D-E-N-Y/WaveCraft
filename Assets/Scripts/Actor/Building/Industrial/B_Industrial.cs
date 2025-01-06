using UnityEngine;

public class B_Industrial : Building
{
    [SerializeField] private E_Resourse resourse;
    
    private void Start() 
    {
        buildingType = E_BuildingType.Industrial;
    }
}
