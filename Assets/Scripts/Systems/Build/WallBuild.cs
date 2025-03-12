using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WallBuild : BaseBuild 
{
    [SerializeField] private GameObject wallPrefab;
    private List<D_Wall> walls;

    public override void Initialize(BuildSystem buildSystem)
    {
        base.Initialize(buildSystem);

        walls = new List<D_Wall>();
    }

    protected override void Update()
    {
        base.Update();

        if(walls.Count >= 1)
        {
            
        }
    }

    protected override void Place(Vector3Int start)
    {
        walls.Add(building.GetComponent<D_Wall>());

        if(walls.Count == 2)
        {
            float totalDistance = Vector3.Distance(walls[0].transform.position, walls[1].transform.position);
            float wallLength = wallPrefab.GetComponent<D_Wall>().GetWallLength();

            float countWalls = (totalDistance - wallLength) / wallLength;
            countWalls = MathF.Round(countWalls);
            
            Vector3 direction = (walls[0].transform.position - walls[1].transform.position).normalized;
            Vector3 middlePosition = (walls[1].transform.position + walls[0].transform.position) / 2;
            Vector3 startPosition = middlePosition + direction * (countWalls * wallLength / 2);

            for (int i = 0; i < countWalls + 1; i++)
            {
                Vector3 spawnPosition = startPosition - direction * (i * wallLength);
            
                GameObject wall = Instantiate(wallPrefab, spawnPosition, Quaternion.identity);
                wall.transform.LookAt(walls[1].transform.position);
            }

            walls.Clear();
        }
    }
}