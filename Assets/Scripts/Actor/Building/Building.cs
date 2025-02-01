using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Building : Actor
{
    [SerializeField] protected float timeToBuild;

    public bool isPlace { private set; get; }   
    public bool isBuild { private set; get; }   

    [SerializeField] protected S_Cost[] cost;
    protected E_BuildingType buildingType;
    
    public Vector3Int Size { private set; get; }
    private Vector3[] Vertices;
    private GridCollider gridCollider;

    protected NavMeshObstacle[] navMeshObstacles;

    public override void Interaction()
    {
        base.Interaction();
    }

    public override void Initialize()
    {
        gridCollider = new GridCollider(gameObject);
        
        Vertices = gridCollider.GetColliderVertexPositionsLocal();
        Size = gridCollider.CalculateSizeInCells();

        isBuild = false;

        navMeshObstacles = GetComponents<NavMeshObstacle>();
    }

    public Vector3 GetStartPosition()
    {
        return transform.TransformPoint(Vertices[0]);
    }

    public void Rotate()
    {
        transform.Rotate(new Vector3(0, 90, 0));
        Size = new Vector3Int(Size.y, Size.x, 1);

        Vector3[] vertices = new Vector3[Vertices.Length];
        for(int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = Vertices[(i + 1) % Vertices.Length];
        }
        
        Vertices = vertices;
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
        foreach(S_Cost amount in cost)
        {
            if(amount.resourse == resource)
            {
                return amount.count;
            }
        }

        return 0;
    }

    public void Place()
    {
        ObjectDrag drag = gameObject?.GetComponent<ObjectDrag>();
        if(drag)
        {
            Destroy(drag);
        }

        isPlace = true;
    }

    public virtual void Built()
    {
        GetComponent<MaterialBuilding>().Built();
        
        foreach(NavMeshObstacle obstacle in navMeshObstacles)
        {
            obstacle.enabled = true;
        }
    }
}
