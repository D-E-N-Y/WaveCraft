using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
            
            float totalDistance = Vector3.Distance(columns[currentPair].transform.position, columns[currentPair - 1].transform.position);
            float wallLength = wallPrefab.GetComponent<D_Wall>().GetWallLength();

            currentCountWalls = (int)MathF.Round((totalDistance - wallLength) / wallLength);
            
            Vector3 direction = (columns[currentPair - 1].transform.position - columns[currentPair].transform.position).normalized;
            Vector3 middlePosition = (columns[currentPair - 1].transform.position + columns[currentPair].transform.position) / 2;
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
                walls[i].transform.LookAt(columns[currentPair].transform.position);
                walls[i].GetComponent<MaterialBuilding>().StartPlace(); 
            }

            for(int i = start; i < countsWall.Last() + currentCountWalls; i++)
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

    public void ContinueWall(D_Wall startColumn)
    {
        columns.Add(startColumn);
        currentPair++;

        InitializeBuilding(columnPrefab);
    }

    protected override void Place()
    {
        if (!buildSystem.CanBePlaced(columns.Last()))
        {
            return;
        }
        
        for(int i = countsWall.Last(); i < countsWall.Last() + currentCountWalls; i++)
        {
            if(!buildSystem.CanBePlaced(walls[i]) && i > countsWall.Last())
            {
                return;
            }
        }

        building.Place();
        buildSystem.PlaceTakeArea(building);
        placedWalls.Add((D_Wall)building);

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

        foreach(D_Wall _wall in placedWalls)
        {
            if(_wall.isBuild) continue;
            
            _wall.Place();

            buildSystem.BusyTakeArea(_wall);

            BuildTask task = new BuildTask(_wall);
            TaskSystem.current.AddTask(task);

            _wall.GetComponent<MaterialBuilding>().SetColor(MaterialBuilding.BuildColor.placed);
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
            int start = countsWall.Last();
            for(int i = start; i < walls.Count; i++)
            {
                walls[i].gameObject.SetActive(false);
            }
            
            countsWall.Remove(countsWall.Last());
            
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

        if(currentPair == -1 || columns[Math.Max(0, currentPair)].isBuild)
        {
            base.Cancel();
            ClearWalls();
            return;
        }
        
        InitializeBuilding(columns[currentPair].gameObject);
    }
}