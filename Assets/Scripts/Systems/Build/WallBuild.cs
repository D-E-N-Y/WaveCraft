using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class WallBuild : BaseBuild 
{
    [SerializeField] private GameObject columnPrefab;
    [SerializeField] private GameObject wallPrefab;
    private List<D_Wall> columns;
    private List<D_Wall> walls;

    private List<D_Wall> placedWalls;

    public override void Initialize(BuildSystem buildSystem)
    {
        base.Initialize(buildSystem);

        columns = new List<D_Wall>();
        walls = new List<D_Wall>();
        placedWalls = new List<D_Wall>();
    }

    private void CreateWalls()
    {
        for (int i = 0; i < 25; i++)
        {
            D_Wall _wall = Instantiate(wallPrefab).GetComponent<D_Wall>();
            _wall.Initialize();
            _wall.gameObject.SetActive(false);
            
            walls.Add(_wall);
        }
    }

    private void ClearWalls()
    {
        foreach(D_Wall _wall in placedWalls)
        {
            walls.Remove(_wall);
        }

        foreach(D_Wall _wall in walls)
        {
            Destroy(_wall);
        }

        walls.Clear();
    }

    protected override void Update()
    {
        base.Update();

        if(columns.Count >= 2)
        {
            foreach(D_Wall _wall in walls)
            {
                _wall.gameObject.SetActive(false);
            }
            
            float totalDistance = Vector3.Distance(columns[0].transform.position, columns[1].transform.position);
            float wallLength = wallPrefab.GetComponent<D_Wall>().GetWallLength();

            float countWalls = (totalDistance - wallLength) / wallLength;
            countWalls = MathF.Round(countWalls);
            
            Vector3 direction = (columns[0].transform.position - columns[1].transform.position).normalized;
            Vector3 middlePosition = (columns[1].transform.position + columns[0].transform.position) / 2;
            Vector3 startPosition = middlePosition + direction * (countWalls * wallLength / 2);
            
            for (int i = 0; i < countWalls + 1; i++)
            {
                Vector3 spawnPosition = startPosition - direction * (i * wallLength);
            
                walls[i].gameObject.SetActive(true);
                walls[i].transform.position = spawnPosition;
                walls[i].transform.LookAt(columns[1].transform.position);
                walls[i].GetComponent<MaterialBuilding>().StartPlace(); 
                
                ViewAreaPlace(walls[i]);
            }

            ViewAreaPlace(columns[0]);
        }
    }

    public override void InitializeBuilding(GameObject prefab)
    {
        base.InitializeBuilding(prefab);

        if(columns.Count == 0)
        {
            CreateWalls();
        }

        columns.Add(building.GetComponent<D_Wall>());
    }

    protected override void Place(Vector3Int start)
    {
        if (columns.Any(x => !buildSystem.CanBePlaced(x)) || walls.Any(x => !buildSystem.CanBePlaced(x)))
        {
            return;
        }
        
        building.Place();
        placedWalls.Add((D_Wall)building);

        if(columns.Count < 2)
        {
            building = null;
            InitializeBuilding(columnPrefab);

            return;
        }
        
        columns[0].GetComponent<MaterialBuilding>().SetColor(MaterialBuilding.BuildColor.placed);
        buildSystem.BusyTakeArea(columns[0]);
        
        BuildTask task = new BuildTask(columns[0]);
        TaskSystem.current.AddTask(task);

        foreach(D_Wall _wall in walls)
        {
            if(_wall.gameObject.activeSelf)
            {
                _wall.Place();

                buildSystem.BusyTakeArea(_wall);

                task = new BuildTask(_wall);
                TaskSystem.current.AddTask(task);

                _wall.GetComponent<MaterialBuilding>().SetColor(MaterialBuilding.BuildColor.placed);
                
                placedWalls.Add(_wall);
            }
            else
            {
                break;
            }
        }

        columns[1].Place();
        columns[1].GetComponent<MaterialBuilding>().SetColor(MaterialBuilding.BuildColor.placed);
        buildSystem.BusyTakeArea(columns[1]);
        
        task = new BuildTask(columns[1]);
        TaskSystem.current.AddTask(task);

        columns.Clear();
        ClearWalls();

        building = null;
        materialBuilding = null;
        
        buildSystem.ActiveTilemap(false);

        enabled = false;
    }
}