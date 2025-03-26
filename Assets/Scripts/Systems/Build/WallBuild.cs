using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class WallBuild : BaseBuild 
{
    [SerializeField] private GameObject columnPrefab;
    [SerializeField] private GameObject wallPrefab;
    private List<D_Wall> columns;
    private List<D_Wall> walls;
    private List<D_Wall> placedWalls;
    
    private int currentPair;
    private bool isContinue;
    private List<int> countsWall;
    private int currentCountWalls;

    public override void Initialize(BuildSystem buildSystem)
    {
        base.Initialize(buildSystem);

        columns = new List<D_Wall>();
        walls = new List<D_Wall>();
        placedWalls = new List<D_Wall>();

        currentPair = -1;
        countsWall = new List<int>();
        countsWall.Add(0);
    }

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
        currentPair = -1;
        currentCountWalls = -1;
    }

    protected override void Update()
    {
        base.Update();

        if(Input.GetKeyDown(KeyCode.Return))
        {
            isContinue = !isContinue;
        }

        if(columns.Count >= 2)
        {
            int start = countsWall[countsWall.Count - 1];
            if(countsWall.Count > 0)
            {
                for(int i = start; i < countsWall[countsWall.Count - 1] + currentCountWalls; i++)
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
            
            float totalDistance = Vector3.Distance(columns[currentPair].transform.position, columns[currentPair - 1].transform.position);
            float wallLength = wallPrefab.GetComponent<D_Wall>().GetWallLength();

            currentCountWalls = (int)MathF.Round((totalDistance - wallLength) / wallLength);
            
            Vector3 direction = (columns[currentPair - 1].transform.position - columns[currentPair].transform.position).normalized;
            Vector3 middlePosition = (columns[currentPair - 1].transform.position + columns[currentPair].transform.position) / 2;
            Vector3 startPosition = middlePosition + direction * (currentCountWalls * wallLength / 2);

            currentCountWalls++;

            for (int i = start ; i < countsWall[countsWall.Count - 1] + currentCountWalls; i++)
            {
                if(walls.Count < i + 1)
                {
                    CreateWall(i);
                }

                Vector3 spawnPosition = startPosition - direction * ((i - countsWall[countsWall.Count - 1]) * wallLength);
            
                walls[i].gameObject.SetActive(true);
                walls[i].transform.position = spawnPosition;
                walls[i].transform.LookAt(columns[currentPair].transform.position);
                walls[i].GetComponent<MaterialBuilding>().StartPlace(); 
            }

            for(int i = start; i < countsWall[countsWall.Count - 1] + currentCountWalls; i++)
            {
                if(walls[i].gameObject.activeSelf)
                {
                    ViewAreaPlace(walls[i], i == start);
                }
            }

            ViewAreaPlace(columns[currentPair]);
        }
    }

    private void ViewAreaPlace(Building _object, bool isConect)
    {
        if(buildSystem.CanBePlaced(_object) || isConect)
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

    public override void InitializeBuilding(GameObject prefab)
    {
        if(!columns.Contains(prefab.GetComponent<D_Wall>()))
        {
            base.InitializeBuilding(prefab);
            columns.Add(building.GetComponent<D_Wall>());

            currentPair++;
        }
        else
        {
            building = prefab.GetComponent<Building>();
            materialBuilding = building.GetComponent<MaterialBuilding>();
            prefab.AddComponent<ObjectDrag>();
        }

        isContinue = true;
    }

    protected override void Place()
    {
        if (!buildSystem.CanBePlaced(columns[columns.Count - 1]))
        {
            return;
        }
        
        for(int i = countsWall[countsWall.Count - 1]; i < countsWall[countsWall.Count - 1] + currentCountWalls; i++)
        {
            if(!buildSystem.CanBePlaced(walls[i]) && i > countsWall[countsWall.Count - 1])
            {
                return;
            }
        }

        building.Place();
        buildSystem.PlaceTakeArea(building);
        placedWalls.Add((D_Wall)building);

        if(isContinue)
        {
            building = null;
            InitializeBuilding(columnPrefab);

            if(currentCountWalls > 0)
            {
                for(int i = countsWall[countsWall.Count - 1]; i < countsWall[countsWall.Count - 1] + currentCountWalls; i++)
                {
                    if(walls[i].gameObject.activeSelf)
                    {
                        buildSystem.PlaceTakeArea(walls[i]);
                    }
                }
                
                countsWall.Add(countsWall[countsWall.Count - 1] + currentCountWalls);  
            }

            return;
        }

        buildSystem.ClearPlacedTilemap();

        for(int i = 0; i < currentPair + 1; i++)
        {
            columns[i].GetComponent<MaterialBuilding>().SetColor(MaterialBuilding.BuildColor.placed);
            buildSystem.BusyTakeArea(columns[i]);
            
            BuildTask task = new BuildTask(columns[i]);
            TaskSystem.current.AddTask(task);
        }

        foreach(D_Wall _wall in walls)
        {
            if(_wall.gameObject.activeSelf)
            {
                _wall.Place();

                buildSystem.BusyTakeArea(_wall);

                BuildTask task = new BuildTask(_wall);
                TaskSystem.current.AddTask(task);

                _wall.GetComponent<MaterialBuilding>().SetColor(MaterialBuilding.BuildColor.placed);
                
                placedWalls.Add(_wall);
            }
        }

        ClearWalls();

        building = null;
        materialBuilding = null;
        
        buildSystem.ActiveTilemap(false);

        enabled = false;
    }

    protected override void Cancel()
    {
        columns[currentPair].gameObject.SetActive(false);
        Destroy(columns[currentPair].gameObject);
        columns.Remove(columns[currentPair]);

        if(countsWall.Count > 1)
        {
            int start = countsWall[countsWall.Count - 1];
            for(int i = start; i < walls.Count; i++)
            {
                walls[i].gameObject.SetActive(false);
            }
            
            countsWall.Remove(countsWall[countsWall.Count - 1]);
            
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
            currentCountWalls = -1;
            walls.ForEach(x => x.gameObject.SetActive(false));
        }

        if(currentPair - 1 != -1) placedWalls.Remove(columns[currentPair - 1]);

        buildSystem.ClearPlacedTilemap();
        placedWalls.ForEach(x => buildSystem.PlaceTakeArea(x));

        currentPair--;

        if(currentPair == -1)
        {
            base.Cancel();
            ClearWalls();
            return;
        }

        InitializeBuilding(columns[currentPair].gameObject);
    }
}