using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class D_Gate : B_Defence
{
    public Action<bool> isPillar;
    
    public EGateState state { get; private set; }

    private List<D_Wall> walls;
    private List<D_Pillar> pillars;

    [SerializeField] private GameObject leftGate, rightGate;
    [SerializeField] private GameObject prop;

    public override string nameActor => "Gate";

    public override void Initialize()
    {
        base.Initialize();

        state = EGateState.open;

        walls = new List<D_Wall>();
        pillars = new List<D_Pillar>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform _actor = other.transform;
        while (true)
        {
            if (_actor.GetComponent<D_Wall>() != null || _actor.GetComponent<D_Pillar>() != null) break;
            if (_actor.parent == null) return;

            _actor = _actor.parent;
        }

        if (_actor.gameObject.TryGetComponent<D_Wall>(out D_Wall _wall))
        {
            if (!walls.Contains(_wall))
            {
                walls.Add(_wall);

                List<D_Wall> _w = walls.Where(x =>
                    Quaternion.Angle(x.transform.rotation, _wall.transform.rotation) < 0.001f)
                    .ToList();

                Quaternion rotationAt180 = Quaternion.Euler(0, transform.eulerAngles.y + 180, 0);

                if (_w.Count > Math.Abs(walls.Count - _w.Count) &&
                   !(Quaternion.Angle(transform.rotation, _wall.transform.rotation) < 0.001f ||
                     Quaternion.Angle(rotationAt180, _wall.transform.rotation) < 0.001f))
                {
                    transform.rotation = _wall.transform.rotation;
                }
            }
        }
        else if (_actor.gameObject.TryGetComponent<D_Pillar>(out D_Pillar _pillar))
        {
            if (!pillars.Contains(_pillar))
            {
                pillars.Add(_pillar);
                isPillar?.Invoke(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Transform _actor = other.transform;
        while (true)
        {
            if (_actor.GetComponent<D_Wall>() != null || _actor.GetComponent<D_Pillar>() != null) break;
            if (_actor.parent == null) return;

            _actor = _actor.parent;
        }

        if (_actor.gameObject.TryGetComponent<D_Wall>(out D_Wall _wall) && walls.Contains(_wall))
        {
            walls.Remove(_wall);
        }
        else if (_actor.gameObject.TryGetComponent<D_Pillar>(out D_Pillar _pillar) && pillars.Contains(_pillar))
        {
            pillars.Remove(_pillar);
            if (!pillars.Any()) isPillar?.Invoke(false);
        }
    }

    public void Close()
    {
        state = EGateState.close;
        
        prop.SetActive(true);
        leftGate.transform.localRotation = Quaternion.identity;
        rightGate.transform.localRotation = Quaternion.identity;
    }

    public void Open()
    {
        state = EGateState.open;

        prop.SetActive(false);
        leftGate.transform.localRotation = Quaternion.Euler(0f, -80f, 0f);
        rightGate.transform.localRotation = Quaternion.Euler(0f, 80f, 0f);
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