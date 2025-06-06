using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WallBuild : BaseBuild 
{
    #region Fields

    [SerializeField] private D_Pillar pillarPrefab;
    [SerializeField] private D_Wall wallPrefab;
    
    private List<D_Pillar> pillars;
    private List<D_Wall> walls;
    private List<Building> placedWalls;
    
    private bool isContinue;
    private List<int> countsWall;
    private int currentCountWalls;

    private int woodCost, stoneCost;
    private int currentWoodCost, currentStoneCost;
    
    #endregion
    
    #region Unity

    protected override void Update()
    {
        base.Update();

        if(Input.GetKeyDown(KeyCode.Return))
        {
            isContinue = !isContinue;
        }

        if (pillars.Count >= 2)
        {
            UpdateWallPreview();
        }

        UpdateCostUI();
    }

    #endregion

    #region Initialization
        
    public override void Initialize(BuildSystem buildSystem, UI_CostBuildingPanel ui_costBuildingPanel)
    {
        base.Initialize(buildSystem, ui_costBuildingPanel);

        pillars = new List<D_Pillar>();
        walls = new List<D_Wall>();
        placedWalls = new List<Building>();

        countsWall = new List<int>();
        countsWall.Add(0);

        woodCost = pillarPrefab.GetCostByResource(E_Resource.Wood);
        stoneCost = pillarPrefab.GetCostByResource(E_Resource.Stone);

        currentWoodCost = currentStoneCost = 0;
    }

    public override void InitializeBuilding(Building prefab)
    {
        if (!pillars.Contains(prefab))
        {
            // take new column
            base.InitializeBuilding(prefab);
            building.gameObject.GetComponent<ObjectDrag>().stopDrag += ConnectColumn;
            pillars.Add((D_Pillar)building);
        }
        else
        {
            // take old column
            building = prefab;
            materialBuilding = building.GetComponent<MaterialBuilding>();
            prefab.gameObject.AddComponent<ObjectDrag>().stopDrag += ConnectColumn;
        }

        isContinue = true;
    }

    #endregion

    #region Wall Creation & Management

    private void CreateWall()
    {
        D_Wall _wall = Instantiate(wallPrefab).GetComponent<D_Wall>();
        _wall.Initialize();
        _wall.gameObject.SetActive(false);

        walls.Add(_wall);
    }

    private void ClearWalls()
    {
        foreach (Building _building in placedWalls)
        {
            if (_building is D_Wall _wall)
            {
                walls.Remove(_wall);
            }
        }

        placedWalls.Clear();

        pillars.Clear();
        countsWall.Clear();

        countsWall.Add(0);
        currentCountWalls = 0;

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
                    CreateWall();
                }
                
                walls[i].gameObject.SetActive(false);
            }
        }
        else
        {
            walls.ForEach(x => x.gameObject.SetActive(false));
        }
        
        float totalDistance = Vector3.Distance(pillars.Last().transform.position, pillars[^2].transform.position);
        float wallLength = wallPrefab.GetComponent<D_Wall>().GetWallLength();

        currentCountWalls = (int)MathF.Round((totalDistance - wallLength) / wallLength);
        
        Vector3 direction = (pillars[^2].transform.position - pillars.Last().transform.position).normalized;
        Vector3 middlePosition = (pillars[^2].transform.position + pillars.Last().transform.position) / 2;
        Vector3 startPosition = middlePosition + direction * (currentCountWalls * wallLength / 2);

        currentCountWalls++;

        for (int i = start ; i < countsWall.Last() + currentCountWalls; i++)
        {
            if(walls.Count < i + 1)
            {
                CreateWall();
            }

            Vector3 spawnPosition = startPosition - direction * ((i - countsWall.Last()) * wallLength);
        
            walls[i].gameObject.SetActive(true);
            walls[i].transform.position = spawnPosition;
            walls[i].transform.LookAt(pillars.Last().transform.position);
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
        ViewAreaPlace(pillars.Last(), isConnect());
    }
        
    #endregion

    #region Resource Handling

     private bool IsCanBuy() => 
        currentWoodCost <= ResourceSystem.current.resources[E_Resource.Wood] && 
        currentStoneCost <= ResourceSystem.current.resources[E_Resource.Stone];

    protected override void UpdateCostUI()
    {
        currentWoodCost = woodCost * (pillars.Count + countsWall.Last() + currentCountWalls);
        currentStoneCost = stoneCost * (pillars.Count + countsWall.Last() + currentCountWalls);

        ui_costBuildingPanel.UpdateCost(currentWoodCost, currentStoneCost);
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
    public void ContinueWall(D_Pillar startPillar)
    {
        pillars.Add(startPillar);
        InitializeBuilding(pillarPrefab);
    }

    private bool isConnect()
    {
        if(pillars.Last().connectPillar)
        {
            if(pillars.Count == 1)
            {
                return true;
            }
            else
            {
                return pillars.Last().connectPillar != pillars[^2];
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
            pillars.Last().transform.position = pillars.Last().connectPillar.transform.position;
        }
    }

    #endregion

    #region Control Walls
    
    protected override void Place()
    { 
        // check corrent land for place current column
        if (!buildSystem.CanBePlaced(pillars.Last()) && !isConnect() || !IsCanBuy())
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

        pillars.Last().gameObject.GetComponent<ObjectDrag>().stopDrag -= ConnectColumn;  
        pillars.Last()?.connectPillar?.ResetConnect();
        
        // place current column
        building.RemoveDrag();
        buildSystem.PlaceTakeArea(building);
        placedWalls.Add(building);

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

        if (isContinue)
        {
            building = null;
            InitializeBuilding(pillarPrefab);

            return;
        }

        buildSystem.ClearPlacedTilemap();

        // place walls
        foreach(Building _wall in placedWalls)
        {
            // skip or destroy unnecessary wall
            if(_wall.isBuild) continue;
            if(_wall is D_Pillar _pillar && _pillar.connectPillar)
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
        
        ResourceSystem.current.RemoveResourceByType(E_Resource.Wood, currentWoodCost);
        ResourceSystem.current.RemoveResourceByType(E_Resource.Stone, currentStoneCost);

        ClearWalls();
        buildSystem.ActiveTilemap(false);
        enabled = false;
    }

    protected override void Cancel()
    {
        // remove last column
        building.gameObject.GetComponent<ObjectDrag>().stopDrag -= ConnectColumn;
        Destroy(pillars.Last().gameObject);
        pillars.Remove(pillars.Last());

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
            currentCountWalls = 0;
            walls.ForEach(x => x.gameObject.SetActive(false));
        }

        // redraw placed tilemap
        if(pillars.Count != 0)
        {
            placedWalls.Remove(pillars.Last());
        } 
        buildSystem.ClearPlacedTilemap();
        placedWalls.ForEach(x => buildSystem.PlaceTakeArea(x));

        if(pillars.Count == 0 || pillars.Last().isBuild)
        {
            base.Cancel();
            ClearWalls();
            return;
        }
        
        // take last column like current
        InitializeBuilding(pillars.Last());
    }

    #endregion
}