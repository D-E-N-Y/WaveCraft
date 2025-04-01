using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GateBuild : BaseBuild 
{
    private D_Gate gate;
    private bool isColumn;

    protected override void Update()
    {
        base.Update();

        ViewAreaPlace(gate);
    }

    public override void InitializeBuilding(GameObject prefab)
    {
        base.InitializeBuilding(prefab);
        
        gate = building.gameObject.GetComponent<D_Gate>();
        gate.gameObject.GetComponent<ObjectDrag>().stopDrag += PlaceBetweenWalls;
        gate.isColumn += SetIsColumn;
    }

    protected override void Rotate()
    {
        gate.transform.Rotate(Vector3.up, 180f);
    }

    private void SetIsColumn(bool value) => isColumn = value;

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
        if(!isColumn && gate.GetOptimalWalls() != null)
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
        if(isColumn || gate.GetOptimalWalls() == null) return;
        
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

        building = null;
        materialBuilding = null;
        gate = null;
        
        buildSystem.ActiveTilemap(false);

        enabled = false;
    }
}