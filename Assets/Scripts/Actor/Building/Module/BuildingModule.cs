using UnityEngine;

public class BuildingModule : MonoBehaviour, IModule
{
    private Building building;

    public virtual void Initialize(Building building)
    {
        this.building = building;
    }

    public Building GetBuilding() => building;
}