using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
        columns = new List<D_Wall>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform _actor = other.transform;
        while(true)
        {
            if(_actor.GetComponent<D_Wall>() != null) break;
            if(_actor.parent == null) return;

            _actor = _actor.parent;
        }
        
        if(_actor.gameObject.TryGetComponent<D_Wall>(out D_Wall _wall))
        {
            if(_wall.Type() == E_WallType.Wall && !walls.Contains(_wall))
            {
                transform.rotation = _wall.transform.rotation;
                walls.Add(_wall);
            }
            else if(_wall.Type() == E_WallType.Column && !columns.Contains(_wall)) 
            {
                columns.Add(_wall);
                isColumn?.Invoke(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Transform _actor = other.transform;
        while(true)
        {
            if(_actor.GetComponent<D_Wall>() != null) break;
            if(_actor.parent == null) return;

            _actor = _actor.parent;
        }        
        
        if(_actor.gameObject.TryGetComponent<D_Wall>(out D_Wall _wall) && 
           (walls.Contains(_wall) || columns.Contains(_wall)))
        {
            if(_wall.Type() == E_WallType.Wall)
            {
                walls.Remove(_wall);
            }
            else if(_wall.Type() == E_WallType.Column)
            {
                columns.Remove(_wall);
                if(!columns.Any()) isColumn?.Invoke(false);
            }
        }
    }

    public List<D_Wall> GetOptimalWalls()
    {
        if(!walls.Any()) return null;

        for (int i = 0; i < walls.Count - 1; i++)
        {
            for(int j = i + 1; j < walls.Count; j++)
            {
                if(Quaternion.Angle(walls[i].transform.rotation, walls[j].transform.rotation) < 0.001f &&
                   (MathF.Round(Vector3.Distance(walls[i].transform.position, walls[j].transform.position)) == walls[i].GetWallLength() || 
                   MathF.Round(Vector3.Distance(walls[i].transform.position, walls[j].transform.position)) == walls[i].GetWallLength() * 3))
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

    public List<D_Wall> GetWallsToRemove()
    {
        if(!walls.Any()) return null;

        for (int i = 0; i < walls.Count - 1; i++)
        {
            for(int j = i + 1; j < walls.Count; j++)
            {
                if(Quaternion.Angle(walls[i].transform.rotation, walls[j].transform.rotation) < 0.001f &&
                   MathF.Round(Vector3.Distance(walls[i].transform.position, walls[j].transform.position)) == walls[i].GetWallLength() &&
                   MathF.Round(Vector3.Distance(transform.position, walls[i].transform.position)) <= walls[i].GetWallLength() &&
                   MathF.Round(Vector3.Distance(transform.position, walls[j].transform.position)) <= walls[i].GetWallLength())
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