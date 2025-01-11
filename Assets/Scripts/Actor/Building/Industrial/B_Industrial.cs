using UnityEngine;

public class B_Industrial : Building
{
    [SerializeField] protected E_Resource resourse;
    
    private void Start() 
    {
        buildingType = E_BuildingType.Industrial;
    }

    public E_Resource GetTypeResource()
    {
        return resourse;
    }
}
