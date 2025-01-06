using System;
using UnityEngine;

public class Building : Actor
{
    [SerializeField] protected int maxHP;
    protected int currentHP;

    [SerializeField] protected float timeToBuild;

    public bool isPlace { private set; get; }   
    public bool isBuild { private set; get; }   

    [SerializeField] protected S_Cost[] cost;
    protected E_BuildingType buildingType;

    public Vector3Int Size { private set; get; }
    private Vector3[] Vertices;

    public void Initialize()
    {
        GetColliderVertexPositionsLocal();
        CalculateSizeInCells();

        isBuild = false;
    }
    
    private void GetColliderVertexPositionsLocal()
    {
        BoxCollider bc = gameObject.GetComponent<BoxCollider>();
        Vertices = new Vector3[4];
        Vertices[0] = bc.center + new Vector3(-bc.size.x, -bc.size.y, -bc.size.z) * 0.5f;
        Vertices[1] = bc.center + new Vector3(bc.size.x, -bc.size.y, -bc.size.z) * 0.5f;
        Vertices[2] = bc.center + new Vector3(bc.size.x, -bc.size.y, bc.size.z) * 0.5f;
        Vertices[3] = bc.center + new Vector3(-bc.size.x, -bc.size.y, bc.size.z) * 0.5f;
    }

    private void CalculateSizeInCells()
    {
        Vector3Int[] vertices = new Vector3Int[Vertices.Length];

        for(int i = 0; i < Vertices.Length; i++)
        {
            Vector3 worldPos = transform.TransformPoint(Vertices[i]);
            vertices[i] = BuildSystem.current.gridLayout.WorldToCell(worldPos);
        }

        Size = new Vector3Int(Math.Abs((vertices[0] - vertices[1]).x), 
                              Math.Abs((vertices[0] - vertices[3]).y), 
                              1);
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

    public virtual void Place()
    {
        ObjectDrag drag = gameObject.GetComponent<ObjectDrag>();
        Destroy(drag);

        isPlace = true;
    }
}
