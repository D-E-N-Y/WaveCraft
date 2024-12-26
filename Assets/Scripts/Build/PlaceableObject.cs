using System;
using UnityEngine;

public class PlaceableObject : MonoBehaviour
{
    public bool Placed { get; private set; }
    public Vector3Int Size { get; private set; }
    private Vector3[] Vertices;

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
            vertices[i] = BuildingSystem.current.gridLayout.WorldToCell(worldPos);
        }

        Size = new Vector3Int(Math.Abs((vertices[0] - vertices[1]).x), 
                              Math.Abs((vertices[0] - vertices[3]).y), 
                              1);
    }

    public Vector3 GetStartPosition()
    {
        return transform.TransformPoint(Vertices[0]);
    }

    private void Start()
    {
        GetColliderVertexPositionsLocal();
        CalculateSizeInCells();
    }

    public virtual void Place()
    {
        ObjectDrag drag = gameObject.GetComponent<ObjectDrag>();
        Destroy(drag);

        Placed = true;

        //invoke events of placement
    }
}
