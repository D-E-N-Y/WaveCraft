using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingSystems : GameSystem
{
    public static BuildingSystems current;
    public Action updateBuildings;

    private List<Building> buildings;

    public override void Initialize()
    {
        current = this;
        buildings = new List<Building>();
    }

    public void AddBuilding(Building building)
    {
        buildings.Add(building);
        updateBuildings?.Invoke();
    }

    public void RemoveBuilding(Building building)
    {
        buildings.Remove(building);
        updateBuildings?.Invoke();
    }

    public List<Building> GetBuildings() => buildings;
    public List<SCircleZone> GetExpansionPlaceZones()
    {
        List<ICircleZone> expansionBuildings = buildings.OfType<ICircleZone>().ToList();
        return expansionBuildings.Select(x => x.GetCircleZone()).ToList();
    }
}