using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingSystems : GameSystem
{
    public static BuildingSystems current;

    private List<Building> buildings;

    public override void Initialize()
    {
        current = this;
        buildings = new List<Building>();
    }

    public void AddBuilding(Building building)
    {
        buildings.Add(building);
    }

    public void RemoveBuilding(Building building)
    {
        buildings.Remove(building);
    }

    public List<Building> GetBuildings() => buildings;
    public List<SCircleZone> GetExpansionPlaceZones()
    {
        return buildings.Select(x => ((ICircleZone)x).GetCircleZone()).ToList();
    }
}