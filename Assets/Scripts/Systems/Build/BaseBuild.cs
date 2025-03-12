using UnityEngine;

public class BaseBuild : MonoBehaviour
{
    protected BuildSystem buildSystem;

    protected Building building;
    protected MaterialBuilding materialBuilding;

    public virtual void Initialize(BuildSystem buildSystem)
    {
        this.buildSystem = buildSystem; 
        enabled = false;
    }

    public virtual void InitializeBuilding(GameObject prefab)
    {
        if(building) return;
        
        Vector3 position = buildSystem.SnapCoordinateToGrid(InteractionSystem.GetMouseWorldPosition());

        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        building = obj.GetComponent<Building>();
        building.Initialize();        
        obj.AddComponent<ObjectDrag>();

        materialBuilding = building.GetComponent<MaterialBuilding>();
        materialBuilding.StartPlace();

        buildSystem.ActiveTilemap(true);
    }

    protected virtual void Update() 
    {
        if(!building) return;

        Vector3Int start = buildSystem.gridLayout.WorldToCell(building.GetStartPosition());
        Move(start);

        if(Input.GetKeyDown(KeyCode.R))
        {
            Rotate();
        }
        else if(Input.GetKeyDown(KeyCode.Space))
        {
            Place(start);
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cancel();
        }
    }

    #region Control
        
    private void Move(Vector3Int start)
    {
        // Grid
        if(!building.isPlace)
        {
            buildSystem.ClearFreeTilemap();
            
            if(buildSystem.CanBePlaced(building))
            {
                materialBuilding.SetColor(MaterialBuilding.BuildColor.canPlace);
                // buildSystem.FreeTakeArea(start, building.Size, buildSystem.CanPlaceTile());
                buildSystem.FreeTakeArea(building, buildSystem.CanPlaceTile());
            }
            else
            {
                materialBuilding.SetColor(MaterialBuilding.BuildColor.notCanPlace);
                // buildSystem.FreeTakeArea(start, building.Size, buildSystem.NotCanPlaceTile());
                buildSystem.FreeTakeArea(building, buildSystem.NotCanPlaceTile());
            }
        }
    }

    protected virtual void Rotate()
    {
        building.Rotate();
    }

    protected virtual void Place(Vector3Int start)
    {
        if(buildSystem.CanBePlaced(building))
        {
            building.Place();
            materialBuilding.SetColor(MaterialBuilding.BuildColor.placed);
            
            buildSystem.BusyTakeArea(building);

            BuildTask task = new BuildTask(building);
            TaskSystem.current.AddTask(task);

            building = null;
            materialBuilding = null;
            
            buildSystem.ActiveTilemap(false);

            enabled = false;
        }
    }
    
    protected virtual void Cancel() 
    {
        Destroy(building.gameObject);
        buildSystem.ActiveTilemap(false);
        enabled = false;
    }

    #endregion
}