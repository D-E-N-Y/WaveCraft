using UnityEngine;
using System;

public class GridCollider
{
    private GameObject current;
    private Vector3Int Size;
    private Vector3[] Vertices;
    
    public GridCollider(GameObject current)
    {
        this.current = current;
    }

    public Vector3[] GetColliderVertexPositionsLocal()
    {
        BoxCollider boxCollider = current.GetComponent<BoxCollider>();
        
        Vertices = new Vector3[4];
        Vertices[0] = boxCollider.center + new Vector3(-boxCollider.size.x, -boxCollider.size.y, -boxCollider.size.z) * 0.5f;
        Vertices[1] = boxCollider.center + new Vector3(boxCollider.size.x, -boxCollider.size.y, -boxCollider.size.z) * 0.5f;
        Vertices[2] = boxCollider.center + new Vector3(boxCollider.size.x, -boxCollider.size.y, boxCollider.size.z) * 0.5f;
        Vertices[3] = boxCollider.center + new Vector3(-boxCollider.size.x, -boxCollider.size.y, boxCollider.size.z) * 0.5f;

        return Vertices;
    }

    public Vector3Int CalculateSizeInCells()
    {
        Vector3Int[] vertices = new Vector3Int[Vertices.Length];

        for(int i = 0; i < Vertices.Length; i++)
        {
            Vector3 worldPos = current.transform.TransformPoint(Vertices[i]);
            vertices[i] = BuildSystem.current.gridLayout.WorldToCell(worldPos);
        }

        Size = new Vector3Int(Math.Abs((vertices[0] - vertices[1]).x), 
                              Math.Abs((vertices[0] - vertices[3]).y), 
                              1);

        return Size;
    }
}