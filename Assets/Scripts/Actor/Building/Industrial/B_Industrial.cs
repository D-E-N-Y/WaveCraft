using UnityEngine;

public abstract class B_Industrial : Building, IIndustrial
{
    [SerializeField] protected E_Resource resource;
    
    private void Start() 
    {
        buildingType = E_BuildingType.Industrial;
    }

    public E_Resource GetTypeResource() => resource;
}
