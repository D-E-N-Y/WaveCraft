using System;
using UnityEngine;
using UnityEngine.AI;

public class Building : Actor
{
    public Action placed, builded;

    [SerializeField] protected float timeToBuild;

    public bool isPlace { private set; get; }
    public bool isBuild { private set; get; }

    [SerializeField] protected S_Cost[] cost;
    protected E_BuildingType buildingType;

    public Vector3Int Size { private set; get; }
    private Vector3[] Vertices;

    protected NavMeshObstacle[] navMeshObstacles;

    public override void Interaction()
    {
        base.Interaction();
    }

    public override void Initialize()
    {
        base.Initialize();

        isBuild = false;

        navMeshObstacles = gameObject.GetComponentsInChildren<NavMeshObstacle>();
    }

    public Vector3 GetStartPosition()
    {
        return transform.TransformPoint(Vertices[0]);
    }

    public float GetTimeToBuild()
    {
        return timeToBuild;
    }

    public S_Cost[] GetCost()
    {
        return cost;
    }

    public int GetCostByResource(E_Resource resource)
    {
        foreach (S_Cost amount in cost)
        {
            if (amount.resourse == resource)
            {
                return amount.count;
            }
        }

        return 0;
    }

    public void Place()
    {
        RemoveDrag();
        isPlace = true;
        placed?.Invoke();
    }

    public void RemoveDrag()
    {
        ObjectDrag drag = gameObject?.GetComponent<ObjectDrag>();
        if (drag)
        {
            Destroy(drag);
        }
    }

    public virtual void Built()
    {
        GetComponent<MaterialBuilding>().Built();
        GetComponent<BoxCollider>().enabled = false;

        foreach (NavMeshObstacle obstacle in navMeshObstacles)
        {
            obstacle.enabled = true;
        }

        isBuild = true;
        builded?.Invoke();
    }

    public virtual void Destroy()
    {
        Death();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        currentHP = Mathf.Max(currentHP - damage, 0);
        UpdateCurrentHP?.Invoke(currentHP);

        if (currentHP <= 0)
        {
            Death();
        }
    }

    protected override void Death()
    {
        GetComponent<BoxCollider>().enabled = enabled;

        BuildSystem.current.ClearBusyTilemap(this);
        BuildSystem.current.RedrawNeighborsBusyArea(this);

        base.Death();
    }
}
