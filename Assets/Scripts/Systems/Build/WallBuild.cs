using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WallBuild : BaseBuild 
{
    #region Fields

    [SerializeField] private UI_CostWallsPanel ui_costWallsPanel;
    [SerializeField] private GameObject columnPrefab;
    [SerializeField] private GameObject wallPrefab;
    
    private List<D_Wall> columns;
    private List<D_Wall> walls;
    private List<D_Wall> placedWalls;
    
    private bool isContinue;
    private List<int> countsWall;
    private int currentCountWalls;

    private int woodCost, stoneCost;
    private int currentWoodCost, currentStoneCost;
    
    #endregion
    
    #region Unity
    
    void OnEnable() => ui_costWallsPanel.gameObject.SetActive(true);
    void OnDisable() => ui_costWallsPanel.gameObject.SetActive(false);

    protected override void Update()
    {
        base.Update();

        if(Input.GetKeyDown(KeyCode.Return))
        {
            isContinue = !isContinue;
        }

        if(columns.Count >= 2)
        {
            UpdateWallPreview();
        }

        UpdateCostUI();
    }

    #endregion

    #region Initialization
        
    public override void Initialize(BuildSystem buildSystem)
    {
        base.Initialize(buildSystem);

        columns = new List<D_Wall>();
        walls = new List<D_Wall>();
        placedWalls = new List<D_Wall>();

        countsWall = new List<int>();
        countsWall.Add(0);

        woodCost = columnPrefab.GetComponent<D_Wall>().GetCostByResource(E_Resource.Wood);
        stoneCost = columnPrefab.GetComponent<D_Wall>().GetCostByResource(E_Resource.Stone);

        currentWoodCost = currentStoneCost = 0;
    }

    public override void InitializeBuilding(GameObject prefab)
    {
        if(!columns.Contains(prefab.GetComponent<D_Wall>()))
        {
            // take new column
            base.InitializeBuilding(prefab);
            building.gameObject.GetComponent<ObjectDrag>().stopDrag += ConnectColumn;
            columns.Add(building.GetComponent<D_Wall>());
        }
        else
        {
            // take old column
            building = prefab.GetComponent<Building>();
            materialBuilding = building.GetComponent<MaterialBuilding>();
            prefab.AddComponent<ObjectDrag>().stopDrag += ConnectColumn;
        }

        isContinue = true;
    }

    #endregion

    #region Wall Creation & Management

    private void CreateWall(int index)
    {
        D_Wall _wall = Instantiate(wallPrefab).GetComponent<D_Wall>();
        _wall.Initialize();
        _wall.gameObject.SetActive(false);

        walls.Add(_wall);
    }

    private void ClearWalls()
    {
        placedWalls.ForEach(x => walls.Remove(x));
        placedWalls.Clear();

        columns.Clear();
        countsWall.Clear();

        countsWall.Add(0);
        currentCountWalls = -1;

        currentWoodCost = currentStoneCost = 0;

        building = null;
        materialBuilding = null;
    }

    private void UpdateWallPreview()
    {
        int start = countsWall.Last();
        if(countsWall.Count > 0)
        {
            for(int i = start; i < countsWall.Last() + currentCountWalls; i++)
            {
                if(walls.Count < i + 1)
                {
                    CreateWall(i);
                }
                
                walls[i].gameObject.SetActive(false);
            }
        }
        else
        {
            walls.ForEach(x => x.gameObject.SetActive(false));
        }
        
        float totalDistance = Vector3.Distance(columns.Last().transform.position, columns[^2].transform.position);
        float wallLength = wallPrefab.GetComponent<D_Wall>().GetWallLength();

        currentCountWalls = (int)MathF.Round((totalDistance - wallLength) / wallLength);
        
        Vector3 direction = (columns[^2].transform.position - columns.Last().transform.position).normalized;
        Vector3 middlePosition = (columns[^2].transform.position + columns.Last().transform.position) / 2;
        Vector3 startPosition = middlePosition + direction * (currentCountWalls * wallLength / 2);

        currentCountWalls++;

        for (int i = start ; i < countsWall.Last() + currentCountWalls; i++)
        {
            if(walls.Count < i + 1)
            {
                CreateWall(i);
            }

            Vector3 spawnPosition = startPosition - direction * ((i - countsWall.Last()) * wallLength);
        
            walls[i].gameObject.SetActive(true);
            walls[i].transform.position = spawnPosition;
            walls[i].transform.LookAt(columns.Last().transform.position);
            walls[i].GetComponent<MaterialBuilding>().StartPlace(); 
        }

        // fill free tilemap
        for(int i = start; i < countsWall.Last() + currentCountWalls; i++)
        {
            if(walls[i].gameObject.activeSelf)
            {
                ViewAreaPlace(walls[i], i == start || i == countsWall.Last() + currentCountWalls - 1);
            }
        }
        ViewAreaPlace(columns.Last(), isConnect());
    }
        
    #endregion

    #region Resource Handling

     private bool IsCanBuy() => 
        currentWoodCost <= ResourceSystem.current.resources[E_Resource.Wood] && 
        currentStoneCost <= ResourceSystem.current.resources[E_Resource.Stone];

    private void UpdateCostUI()
    {
        currentWoodCost = woodCost * (columns.Count + countsWall.Last() + currentCountWalls);
        currentStoneCost = stoneCost * (columns.Count + countsWall.Last() + currentCountWalls);

        ui_costWallsPanel.UpdateCost(currentWoodCost, currentStoneCost);
    }
        
    #endregion

    #region Utility

    private void ViewAreaPlace(Building _object, bool isConnect)
    {
        if((buildSystem.CanBePlaced(_object) || isConnect) && IsCanBuy())
        {
            // can place (green tiles)
            _object.GetComponent<MaterialBuilding>().SetColor(MaterialBuilding.BuildColor.canPlace);
            buildSystem.FreeTakeArea(_object, buildSystem.CanPlaceTile());
        }
        else
        {
            // can't place (red tiles)
            _object.GetComponent<MaterialBuilding>().SetColor(MaterialBuilding.BuildColor.notCanPlace);
            buildSystem.FreeTakeArea(_object, buildSystem.NotCanPlaceTile());
        }
    }
    
    // start built from built column
    public void ContinueWall(D_Wall startColumn)
    {
        columns.Add(startColumn);
        InitializeBuilding(columnPrefab);
    }

    private bool isConnect()
    {
        if(columns.Last().connectColumn)
        {
            if(columns.Count == 1)
            {
                return true;
            }
            else
            {
                return columns.Last().connectColumn != columns[^2];
            }
        }
        else
        {
            return false;
        }
    }
    
    private void ConnectColumn()
    {
        if(isConnect())
        {
            columns.Last().transform.position = columns.Last().connectColumn.transform.position;
        }
    }

    #endregion

    #region Control Walls
    
    protected override void Place()
    {
        // check corrent land for place current column
        if (!buildSystem.CanBePlaced(columns.Last()) && !isConnect() || !IsCanBuy())
        {
            return;
        }
        
        // check corrent land for place current walls
        for(int i = countsWall.Last(); i < countsWall.Last() + currentCountWalls; i++)
        {
            if(!buildSystem.CanBePlaced(walls[i]) && i > countsWall.Last() && i < countsWall.Last() + currentCountWalls - 1 || !IsCanBuy())
            {
                return;
            }
        }

        columns.Last().gameObject.GetComponent<ObjectDrag>().stopDrag -= ConnectColumn;  
        columns.Last()?.connectColumn?.ResetConnect();
        
        // place current column
        building.Place();
        buildSystem.PlaceTakeArea(building);
        placedWalls.Add((D_Wall)building);

        // add walls to placed walls list
        if(currentCountWalls > 0)
        {
            for(int i = countsWall.Last(); i < countsWall.Last() + currentCountWalls; i++)
            {
                if(walls[i].gameObject.activeSelf)
                {
                    buildSystem.PlaceTakeArea(walls[i]);
                    placedWalls.Add(walls[i]);
                }
            }
            
            countsWall.Add(countsWall.Last() + currentCountWalls);  
        }

        if(isContinue)
        {
            building = null;
            InitializeBuilding(columnPrefab);

            return;
        }

        buildSystem.ClearPlacedTilemap();

        // place walls
        foreach(D_Wall _wall in placedWalls)
        {
            // skip or destroy unnecessary wall
            if(_wall.isBuild) continue;
            if(_wall.connectColumn)
            {
                Destroy(_wall.gameObject);
                continue;
            }

            _wall.Place();

            // fill busy tilemap
            buildSystem.BusyTakeArea(_wall);

            // create task for built wall
            BuildTask task = new BuildTask(_wall);
            TaskSystem.current.AddTask(task);

            // set material
            _wall.GetComponent<MaterialBuilding>().SetColor(MaterialBuilding.BuildColor.placed);
        }
        
        ResourceSystem.current.RemoveResources(E_Resource.Wood, currentWoodCost);
        ResourceSystem.current.RemoveResources(E_Resource.Stone, currentStoneCost);

        ClearWalls();
        buildSystem.ActiveTilemap(false);
        enabled = false;
    }

    protected override void Cancel()
    {
        // remove last column
        building.gameObject.GetComponent<ObjectDrag>().stopDrag -= ConnectColumn;
        Destroy(columns.Last().gameObject);
        columns.Remove(columns.Last());

        if(countsWall.Count > 1)
        {
            // deactive current walls
            int start = countsWall.Last();
            for(int i = start; i < walls.Count; i++)
            {
                walls[i].gameObject.SetActive(false);
            }
            countsWall.Remove(countsWall.Last());
            
            // take last placed walls like current walls
            start = countsWall[countsWall.Count - 1];
            for(int i = start; i < walls.Count; i++)
            {
                if(placedWalls.Contains(walls[i]))
                {
                    placedWalls.Remove(walls[i]);
                }
            }
        }
        else
        {
            // deactive all walls
            currentCountWalls = -1;
            walls.ForEach(x => x.gameObject.SetActive(false));
        }

        // redraw placed tilemap
        if(columns.Count != 0)
        {
            placedWalls.Remove(columns.Last());
        } 
        buildSystem.ClearPlacedTilemap();
        placedWalls.ForEach(x => buildSystem.PlaceTakeArea(x));

        if(columns.Count == 0 || columns.Last().isBuild)
        {
            base.Cancel();
            ClearWalls();
            return;
        }
        
        // take last column like current
        InitializeBuilding(columns.Last().gameObject);
    }

    #endregion
}