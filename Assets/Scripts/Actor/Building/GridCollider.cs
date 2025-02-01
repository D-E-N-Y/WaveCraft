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
        
        // if(collider is BoxCollider boxCollider)
        // {
        //     Vertices = new Vector3[4];
        //     Vertices[0] = boxCollider.center + new Vector3(-boxCollider.size.x, -boxCollider.size.y, -boxCollider.size.z) * 0.5f;
        //     Vertices[1] = boxCollider.center + new Vector3(boxCollider.size.x, -boxCollider.size.y, -boxCollider.size.z) * 0.5f;
        //     Vertices[2] = boxCollider.center + new Vector3(boxCollider.size.x, -boxCollider.size.y, boxCollider.size.z) * 0.5f;
        //     Vertices[3] = boxCollider.center + new Vector3(-boxCollider.size.x, -boxCollider.size.y, boxCollider.size.z) * 0.5f;
        // }
        // else if(collider is CapsuleCollider capsuleCollider)
        // {
        //     Vertices = new Vector3[4];

        //     if (capsuleCollider.direction == 0)  // Ось X
        //     {
        //         Vertices[0] = capsuleCollider.center + new Vector3(-capsuleCollider.radius, 0, 0);
        //         Vertices[1] = capsuleCollider.center + new Vector3(capsuleCollider.radius, 0, 0);
        //         Vertices[2] = capsuleCollider.center + new Vector3(-capsuleCollider.radius, 0, capsuleCollider.height * 0.5f);
        //         Vertices[3] = capsuleCollider.center + new Vector3(capsuleCollider.radius, 0, capsuleCollider.height * 0.5f);
        //     }
        //     else if (capsuleCollider.direction == 1)  // Ось Y
        //     {
        //         Vertices[0] = capsuleCollider.center + new Vector3(0, -capsuleCollider.radius, 0);
        //         Vertices[1] = capsuleCollider.center + new Vector3(0, capsuleCollider.radius, 0);
        //         Vertices[2] = capsuleCollider.center + new Vector3(0, -capsuleCollider.radius, capsuleCollider.height * 0.5f);
        //         Vertices[3] = capsuleCollider.center + new Vector3(0, capsuleCollider.radius, capsuleCollider.height * 0.5f);
        //     }
        //     else if (capsuleCollider.direction == 2)  // Ось Z
        //     {
        //         Vertices[0] = capsuleCollider.center + new Vector3(0, 0, -capsuleCollider.radius);
        //         Vertices[1] = capsuleCollider.center + new Vector3(0, 0, capsuleCollider.radius);
        //         Vertices[2] = capsuleCollider.center + new Vector3(0, capsuleCollider.height * 0.5f, -capsuleCollider.radius);
        //         Vertices[3] = capsuleCollider.center + new Vector3(0, capsuleCollider.height * 0.5f, capsuleCollider.radius);
        //     }
        // }  

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