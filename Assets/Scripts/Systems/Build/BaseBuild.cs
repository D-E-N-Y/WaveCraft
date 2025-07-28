using UnityEngine;

public class BaseBuild : MonoBehaviour
{
    protected BuildSystem buildSystem;

    protected Building building;
    protected MaterialBuilding materialBuilding;

    protected UI_CostBuildingPanel ui_costBuildingPanel;

    void OnEnable() => ui_costBuildingPanel.gameObject.SetActive(true);
    void OnDisable() => ui_costBuildingPanel.gameObject.SetActive(false);

    public virtual void Initialize(BuildSystem buildSystem, UI_CostBuildingPanel ui_costBuildingPanel)
    {
        this.buildSystem = buildSystem;
        this.ui_costBuildingPanel = ui_costBuildingPanel;
        enabled = false;
    }

    public virtual void InitializeBuilding(Building prefab)
    {
        if (building) return;

        Vector3 position = buildSystem.SnapCoordinateToGrid(InteractionSystem.GetMouseWorldPosition());

        building = Instantiate(prefab, position, Quaternion.identity);
        building.Initialize();
        building.gameObject.AddComponent<ObjectDrag>();

        materialBuilding = building.GetComponent<MaterialBuilding>();
        materialBuilding.StartPlace();

        buildSystem.ActiveTilemap(true);
        buildSystem.ActivePlaceZone(true);
        UpdateCostUI();
    }

    protected virtual void Update()
    {
        if (!building) return;

        buildSystem.ClearFreeTilemap();
        ViewAreaPlace(building);

        if (Input.GetKeyDown(KeyCode.R))
        {
            Rotate();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Place();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cancel();
        }
    }

    #region Control

    protected void ViewAreaPlace(Building _object)
    {
        if (buildSystem.CanBePlaced(_object))
        {
            _object.GetComponent<MaterialBuilding>().SetColor(MaterialBuilding.BuildColor.canPlace);
            buildSystem.FreeTakeArea(_object, buildSystem.CanPlaceTile());
        }
        else
        {
            _object.GetComponent<MaterialBuilding>().SetColor(MaterialBuilding.BuildColor.notCanPlace);
            buildSystem.FreeTakeArea(_object, buildSystem.NotCanPlaceTile());
        }
    }

    protected virtual void Rotate()
    {
        building.transform.Rotate(Vector3.up, 15f);
    }

    protected virtual void Place()
    {
        if (buildSystem.CanBePlaced(building))
        {
            building.Place();
            materialBuilding.SetColor(MaterialBuilding.BuildColor.placed);

            buildSystem.BusyTakeArea(building);

            BuildTask task = new BuildTask(building);
            TaskSystem.current.AddTask(task);

            ResourceSystem.current.RemoveResources(building.GetCost());

            building = null;
            materialBuilding = null;

            buildSystem.ActiveTilemap(false);
            buildSystem.ActivePlaceZone(false);

            enabled = false;
        }
    }

    protected virtual void Cancel()
    {
        Destroy(building.gameObject);
        buildSystem.ActiveTilemap(false);
        buildSystem.ActivePlaceZone(false);
        enabled = false;
    }

    protected virtual void UpdateCostUI()
    {
        ui_costBuildingPanel.UpdateCost(building.GetCostByResource(E_Resource.Wood), building.GetCostByResource(E_Resource.Stone));
    }

    public bool HasBuilding() => building;

    #endregion
}