using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class D_Gate : B_Defence
{
    public Action<bool> isColumn;

    private List<D_Wall> walls;
    private List<D_Wall> columns;

    public override void Initialize()
    {
        base.Initialize();

        nameActor = "Gate";
        walls = new List<D_Wall>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<D_Wall>(out D_Wall _wall))
        {
            if(_wall.Type() == E_WallType.Wall)
            {
                walls.Add(_wall);
            }
            else
            {
                columns.Add(_wall);
                isColumn?.Invoke(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.TryGetComponent<D_Wall>(out D_Wall _wall) && 
           (walls.Contains(_wall) || columns.Contains(_wall)))
        {
            if(_wall.Type() == E_WallType.Wall)
            {
                walls.Remove(_wall);
            }
            else
            {
                columns.Remove(_wall);
                if(!columns.Any()) isColumn?.Invoke(false);
            }
        }
    }

    public List<D_Wall> GetOptimalWalls()
    {
        if(!walls.Any()) return null;

        for (int i = 0; i < walls.Count; i++)
        {
            for(int j = i + 1; j < walls.Count - 1; j++)
            {
                if(walls[i].transform.rotation.y == walls[j].transform.rotation.y &&
                   Vector3.Distance(walls[i].transform.position, walls[j].transform.position) == walls[i].GetWallLength() * 3)
                {
                    List<D_Wall> optimal = new List<D_Wall>();
                    optimal.Add(walls[i]);
                    optimal.Add(walls[j]);

                    return optimal;
                }
            }
        }

        return null;
    }
}