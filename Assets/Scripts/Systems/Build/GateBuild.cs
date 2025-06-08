using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GateBuild : BaseBuild 
{
    private D_Gate gate;
    private bool isPillar;

    protected override void Update()
    {
        base.Update();

        ViewAreaPlace(gate);
    }

    public override void InitializeBuilding(Building prefab)
    {
        base.InitializeBuilding(prefab);
        
        gate = (D_Gate)building;
        gate.gameObject.GetComponent<ObjectDrag>().stopDrag += PlaceBetweenWalls;
        gate.isPillar += SetIsPillar;
    }

    protected override void Rotate()
    {
        gate.transform.Rotate(Vector3.up, 180f);
    }

    private void SetIsPillar(bool value) => isPillar = value;

    private void PlaceBetweenWalls()
    {
        if(gate.GetOptimalWalls() != null)
        {
            List<D_Wall> _walls = gate.GetOptimalWalls();
            gate.transform.position =( _walls[0].transform.position + _walls[1].transform.position) / 2;
        }
    }

    private new void ViewAreaPlace(Building _object)
    {
        if(!isPillar && gate.GetOptimalWalls() != null)
        {
            // can place (green tiles)
            materialBuilding.SetColor(MaterialBuilding.BuildColor.canPlace);
            buildSystem.FreeTakeArea(_object, buildSystem.CanPlaceTile());
        }
        else
        {
            // can't place (red tiles)
            materialBuilding.SetColor(MaterialBuilding.BuildColor.notCanPlace);
            buildSystem.FreeTakeArea(_object, buildSystem.NotCanPlaceTile());
        }
    }

    protected override void Place()
    {
        if(isPillar || gate.GetOptimalWalls() == null) return;
        
        building.Place();
        materialBuilding.SetColor(MaterialBuilding.BuildColor.placed);
        
        buildSystem.BusyTakeArea(building);

        List<D_Wall> wallsToRemove = gate.GetWallsToRemove();
        if(wallsToRemove != null)
        {
            foreach(D_Wall _wall in wallsToRemove)
            {
                if(_wall.isBuild)
                {
                    DestroyTask _task = new DestroyTask(_wall);
                    TaskSystem.current.AddTask(_task);
                }
                else
                {
                    _wall.gameObject.SetActive(false);
                }
            }
        }

        BuildTask task = new BuildTask(building);
        TaskSystem.current.AddTask(task);

        ResourceSystem.current.RemoveResources(building.GetCost());

        building = null;
        materialBuilding = null;
        gate = null;
        
        buildSystem.ActiveTilemap(false);

        enabled = false;
    }
}